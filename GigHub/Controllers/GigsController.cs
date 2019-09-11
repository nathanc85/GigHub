using GigHub.Models;
using GigHub.Repositories;
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
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;
        private readonly GenreRepository _genreRepository;
        private readonly FollowingRepository _followingRepository;
        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
            _genreRepository = new GenreRepository(_context);
            _followingRepository = new FollowingRepository(_context);
        }
        [Authorize]
        public ActionResult Mine()
        {
            var currentId = User.Identity.GetUserId();
            var gigs = _gigRepository.GetFutureGigsWithGenre(currentId);

            return View(gigs);

        }

        [Authorize]
        public ActionResult Attending()
        {
            var currentUser = User.Identity.GetUserId();

            // Get all the gigs the current user is attending.

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = _gigRepository.GetGigsUserAttending(currentUser),
                Attendances = _attendanceRepository.GetFutureAttendances(currentUser).ToLookup(a => a.GigId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };

            return View("Gigs", viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        public ActionResult Create()
        {
            GigFormViewModel viewModel = new GigFormViewModel
            {
                Genres = _genreRepository.GetGenres(),
                Heading = "Create gig"
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _genreRepository.GetGenres();
                return View("GigForm", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                Venue = viewModel.Venue,
                GenreId = viewModel.Genre,
            };

            _gigRepository.Add(gig);
            _gigRepository.Submit();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var currentUser = User.Identity.GetUserId();
            var gig = _gigRepository.GetGig(id);

            if (gig == null)
            {
                return HttpNotFound();
            }

            if (gig.ArtistId != currentUser)
            {
                return new HttpUnauthorizedResult();
            }
            GigFormViewModel viewModel = new GigFormViewModel
            {
                Genres = _genreRepository.GetGenres(),
                Id = gig.Id,
                Date = gig.DateTime.ToString("MM/dd/yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _genreRepository.GetGenres();
                return View("GigForm", viewModel);
            }

            var currentUser = User.Identity.GetUserId();
            var gig = _gigRepository.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
            {
                return HttpNotFound();
            }

            if (gig.ArtistId != currentUser)
            {
                return new HttpUnauthorizedResult();
            }

            // Update and send update notifications.
            gig.Update(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _gigRepository.Submit();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult GigDetails(int id)
        {
            var gig = _gigRepository.GetGigWithArtistAndGenre(id);

            if (gig == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var currentUser = User.Identity.GetUserId();

                // Find out if the user is going.
                viewModel.IsAttending = _attendanceRepository.UserIsAttendingGig(gig.Id, currentUser);

                // Find out if the user is following the artist.
                viewModel.IsFollowing = _followingRepository.UserFollowingArtist(gig.ArtistId, currentUser);
            }

            return View("GigDetails", viewModel);
        }
    }
}