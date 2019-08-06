using System.Collections.Generic;
using System.Linq;
using GigHub.Models;

namespace GigHub.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Gig> upcomingGigs { get; set; }
        public bool ShowActions { get; set; }
    }
}