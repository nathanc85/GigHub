﻿using GigHub.Models;
using System.Data.Entity;
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
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id && g.ArtistId == currentId);

            // If the gig is flagged as Canceled then return NotFound.
            if (gig.IsCanceled)
            {
                return NotFound();
            }
            gig.IsCanceled = true;

            // Create the notification for the gig being cancelled.
            var notification = new Notification(NotificationType.GigCanceled, gig);

            // Loop throught all the attenndees and create UserNotifications
            // aka instances of the notification for each one of them.
            foreach(var attendee in gig.Attendances.Select(a => a.Attendee)
            {
                attendee.Notify(notification);
            }

            _context.SaveChanges();
            return Ok();
        }
    }
}
