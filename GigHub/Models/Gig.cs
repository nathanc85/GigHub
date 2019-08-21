using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public bool IsCanceled { get; private set; }
        public ApplicationUser Artist { get; set; }
        [Required]
        public string ArtistId { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        [StringLength(255)]
        public string Venue { get; set; }
        public Genre Genre { get; set; }
        [Required]
        public byte GenreId { get; set; }
        public ICollection<Attendance> Attendances{ get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;

            // Create the notification for the gig being cancelled.
            var notification = new Notification(NotificationType.GigCanceled, this);

            // Loop throught all the attenndees and create UserNotifications
            // aka instances of the notification for each one of them.
            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}