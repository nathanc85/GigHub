using System;
using System.Collections.Generic;
using System.Data.Entity;
using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Test.Persistence.Repositories
{
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;
        private Mock<DbSet<Attendance>> _mockAttendances;
        [TestInitialize]
        public void TestInitialize()
        {
            var mockContext = new Mock<IApplicationDbContext>();

            _mockGigs = new Mock<DbSet<Gig>>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);

            _mockAttendances = new Mock<DbSet<Attendance>>();
            mockContext.SetupGet(c => c.Attendances).Returns(_mockAttendances.Object);

            _repository = new GigRepository(mockContext.Object);
        }

        // Tests for GetGigWithArtistAndGenre.
        [TestMethod]
        public void GetFutureGigsWithGenre_GigIsInThePast_ShouldNotBeReturned()
        {
            // Arrange
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(-1), ArtistId = "1" };
            _mockGigs.SetSource(new List<Gig> { gig });

            // Act
            var gigs = _repository.GetFutureGigsWithGenre("1");

            // Assert
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureGigsWithGenre_GigIsCancelled_ShouldNotBeReturned()
        {
            // Arrange
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();
            _mockGigs.SetSource(new List<Gig> { gig });

            // Act
            var gigs = _repository.GetFutureGigsWithGenre("1");

            // Assert
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureGigsWithGenre_GigIsFromDifferentArtist_ShouldNotBeReturned()
        {
            // Arrange
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            _mockGigs.SetSource(new List<Gig> { gig });

            // Act
            var gigs = _repository.GetFutureGigsWithGenre(gig.ArtistId + "-");

            // Assert
            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetFutureGigsWithGenre_GigIsFromTheGivenArtistAndIsFromTheFuture_ShouldBeReturned()
        {
            // Arrange
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            _mockGigs.SetSource(new List<Gig> { gig });

            // Act
            var gigs = _repository.GetFutureGigsWithGenre("1");

            // Assert
            gigs.Should().HaveCount(1);
        }

        // Tests for GetGigsUserAttending.
        [TestMethod]
        public void GetGigsUserAttending_GigIsFromDifferentArtist_ShouldNotBeReturned()
        {
            // Arrange
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1)};
            var attendance = new Attendance() { Gig = gig, AttendeeId = "1" };

            _mockAttendances.SetSource(new List<Attendance> { attendance });

            // Act
            var attendances = _repository.GetGigsUserAttending(attendance.AttendeeId + "-");

            // Assert
            attendances.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserAttending_GigIsFromThePast_ShouldNotBeReturned()
        {
            // Arrange
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(-1)};
            var attendance = new Attendance() { Gig = gig, AttendeeId = "1" };

            _mockAttendances.SetSource(new List<Attendance> { attendance });

            // Act
            var attendances = _repository.GetGigsUserAttending(attendance.AttendeeId);

            // Assert
            attendances.Should().BeEmpty();
        }
    }
}
