using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.App_Start;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var currentUser = User.Identity.GetUserId();
            var notifications = _unitOfWork.Notifications.GetNewNotificationsFor(currentUser);

            var mapper = MappingConfig.GetMapper();
            return notifications.Select(mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult SeAsRead()
        {
            var currentUser = User.Identity.GetUserId();

            var notifications = _unitOfWork.UserNotifications.GetUserNotificationsFor(currentUser);

            //foreach (var userNotification in userNotifications)
            //{
            //    userNotification.IsRead = true;
            //}

            notifications.ForEach(un => un.Read());
            _unitOfWork.Complete();
            return Ok();
        }
    }
}
