using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            // Get the current logged in user.
            var currentId = User.Identity.GetUserId();

            // Find the gig.
            var gig = _context.Gigs
                .Single(g => g.Id == id && g.ArtistId == currentId);

            // If the gig is flagged as Canceled then return NotFound.
            if (gig.IsCanceled)
            {
                return NotFound();
            }
            gig.IsCanceled = true;

            // Create the notification for the gig being cancelled.
            var notification = new Notification()
            {
                DateTime = DateTime.Now,
                Gig = gig,
                NotificationType = NotificationType.GigCanceled
            };

            // Get a list of all the attendees that will get the notification.
            var attendees = _context.Attendances
                .Where(a => a.GigId == gig.Id)
                .Select(u => u.Attendee)
                .ToList();

            // Loop throught all the attenndees and create UserNotifications
            // aka instances of the notification for each one of them.
            foreach(var attendee in attendees)
            {
                var userNotification = new UserNotification
                {
                    User = attendee,
                    Notification = notification
                };

                _context.UserNotifications.Add(userNotification);
            }

            _context.SaveChanges();
            return Ok();
        }
    }
}
