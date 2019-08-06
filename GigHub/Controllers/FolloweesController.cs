using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class FolloweesController : Controller
    {
        ApplicationDbContext _context;

        public FolloweesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Followees
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();

            var followees = _context.Followings
                .Where(f => f.FollowerId == currentUser)
                .Select(u => u.Followee)
                .ToList();

            return View(followees);
        }
    }
}