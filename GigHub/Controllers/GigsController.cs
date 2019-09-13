using GigHub.Models;
using GigHub.Persistence;
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
        private readonly IUnitOfWork _unitOfWork;
        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        public ActionResult Mine()
        {
            var currentId = User.Identity.GetUserId();
            var gigs = _unitOfWork.Gigs.GetFutureGigsWithGenre(currentId);

            return View(gigs);

        }

        [Authorize]
        public ActionResult Attending()
        {
            var currentUser = User.Identity.GetUserId();

            // Get all the gigs the current user is attending.

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(currentUser),
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(currentUser).ToLookup(a => a.GigId),
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
                Genres = _unitOfWork.Genres.GetGenres(),
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                Venue = viewModel.Venue,
                GenreId = viewModel.Genre,
            };

            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var currentUser = User.Identity.GetUserId();
            var gig = _unitOfWork.Gigs.GetGig(id);

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
                Genres = _unitOfWork.Genres.GetGenres(),
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", viewModel);
            }

            var currentUser = User.Identity.GetUserId();
            var gig = _unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);

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

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult GigDetails(int id)
        {
            var gig = _unitOfWork.Gigs.GetGigWithArtistAndGenre(id);

            if (gig == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var currentUser = User.Identity.GetUserId();

                // Find out if the user is going.
                viewModel.IsAttending = _unitOfWork.Attendances.UserIsAttendingGig(gig.Id, currentUser);

                // Find out if the user is following the artist.
                viewModel.IsFollowing = _unitOfWork.Followings.UserFollowingArtist(gig.ArtistId, currentUser);
            }

            return View("GigDetails", viewModel);
        }
    }
}