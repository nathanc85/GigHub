using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Persistence;
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
        //ApplicationDbContext _context;
        IUnitOfWork _unitOfWork;

        public FolloweesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_context = new ApplicationDbContext();
        }
        // GET: Followees
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();

            var followees = _unitOfWork.Followings.GetFollowees(currentUser);
           
            return View(followees);
        }
    }
}