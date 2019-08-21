﻿using GigHub.Models;
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
        public ActionResult Index()
        {
            var isAuth = User.Identity.IsAuthenticated;

            var upcomingGigs = _context.Gigs.Include(g => g.Artist)
            .Include(g => g.Genre)
            .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);

            var viewModel = new GigsViewModel
            {
                upcomingGigs = upcomingGigs,
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