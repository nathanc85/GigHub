using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using GigHub.Controllers;
using GigHub.Persistence;
using System.Linq;
using GigHub.IntegrationTest.Extensions;
using GigHub.Core.Models;
using System.Web.Mvc;
using System.Collections;
using FluentAssertions;
using GigHub.Core.ViewModels;

namespace GigHub.IntegrationTest.Controllers
{
    [TestFixture]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private ApplicationDbContext _context;
        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            // We need to pass a real Unit of Work that talks to a database, so no mocking it.
            _controller = new GigsController(new UnitOfWork(_context));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void Mine_WhenCalled_ShouldReturnUpcomingGigs()
        {
            // Arrange
            // Mock the current user --> use the extension we created.
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            // Create a new Gig.
            var genre = _context.Genres.First();
            var gig = new Gig { Artist = user, DateTime = DateTime.Now.AddDays(2), Genre = genre, Venue = "-" };
            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Act
            var result = _controller.Mine() as ViewResult;

            // Assert
            (result.ViewData.Model as IEnumerable<Gig>)?.Should().HaveCount(1);
        }

        [Test, Isolated]
        public void Update_WhenCalled_ShouldUpdateTheGig()
        {
            // Arrange
            // Mock the current user --> use the extension we created.
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            // Create a new Gig.
            var genre = _context.Genres.Single( g => g.Id == 1);
            var gig = new Gig { Artist = user, DateTime = DateTime.Now.AddDays(2), Genre = genre, Venue = "-" };
            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Act
            var result = _controller.Update(new GigFormViewModel
                {
                    Id = gig.Id,
                    Venue = "San Jose Teather",
                    Date = DateTime.Today.AddMonths(3).ToString("MM/dd/yyyy"),
                    Time = "20:00",
                    Genre = 2

            });

            // Assert
            _context.Entry(gig).Reload();
            gig.DateTime.Should().Be(DateTime.Today.AddMonths(3).AddHours(20));
            gig.Venue.Should().Be("San Jose Teather");
            gig.GenreId.Should().Be(2);
        }
    }
}
