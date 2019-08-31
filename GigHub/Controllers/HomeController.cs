using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index(string query = null)
        {
            var isAuth = User.Identity.IsAuthenticated;
            var currentUser = User.Identity.GetUserId();

            // Get all the upcoming gigs.
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);

            // Filter based on the query search.
            if (!String.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs
                    .Where(u => u.Artist.Name.Contains(query)
                    || u.Genre.Name.Contains(query)
                    || u.Venue.Contains(query));

            }

            // Get all the gigs the current user is attending.
            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == currentUser && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            // Create the model.
            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs,
                Attendances = attendances,
                ShowActions = isAuth,
                Heading = "Upcoming Gigs"
            };

            return View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}