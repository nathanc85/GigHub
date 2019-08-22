using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var currentUser = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == currentUser)
                .Select(un => un.Notification)
                .Include(g => g.Gig.Artist)
                .ToList();

            return notifications.Select(n => new NotificationDto()
            {
                DateTime = n.DateTime,
                NotificationType = n.NotificationType,
                OriginalDateTime = n.OriginalDateTime,
                OriginalVenue = n.OriginalVenue,
                Gig = new GigDto()
                {
                    Id = n.Gig.Id,
                    IsCanceled = n.Gig.IsCanceled,
                    DateTime = n.Gig.DateTime,
                    Venue = n.Gig.Venue,
                    Artist = new UserDto()
                    {
                        Id = n.Gig.Artist.Id,
                        Name = n.Gig.Artist.Name
                    }
                }
            });
        }
    }
}
