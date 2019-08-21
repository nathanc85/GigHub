﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        [Required]
        public Gig Gig { get; private set; }

        private Notification()
        {

        }

        private  Notification(NotificationType notificationType, Gig gig)
        {
            if (gig == null)
            {
                throw new ArgumentNullException("gig");
            }
            DateTime = DateTime.Now;
            NotificationType = notificationType;
            Gig = gig;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue)
        {
            var notification = new Notification(NotificationType.GigUpdated, newGig);
            notification.OriginalDateTime = originalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }
    }
}