using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class Notification {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }

        [Required]
        public Gig Gig { get; private set; }

        private Notification()
        {

        }

        public Notification(NotificationType notificationType, Gig gig)
        {
            if (gig == null)
            {
                throw new ArgumentNullException("gig");
            }
            DateTime = DateTime.Now;
            NotificationType = notificationType;
            Gig = gig;
        }
    }
}