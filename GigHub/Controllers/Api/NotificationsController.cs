using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.App_Start;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var currentUser = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == currentUser && !un.IsRead)
                .Select(un => un.Notification)
                .Include(g => g.Gig.Artist)
                .ToList();

            var mapper = MappingConfig.GetMapper();
            return notifications.Select(mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult SeAsRead()
        {
            var currentUser = User.Identity.GetUserId();

            var userNotifications = _context.UserNotifications
                                        .Where(un => un.UserId == currentUser && !un.IsRead)
                                        .ToList();

            //foreach (var userNotification in userNotifications)
            //{
            //    userNotification.IsRead = true;
            //}

            userNotifications.ForEach(un => un.Read());
            _context.SaveChanges();
            return Ok();
        }
    }
}
