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
        [TestInitialize]
        public void TestInitialize()
        {
            var mockContext = new Mock<IApplicationDbContext>();
            _mockGigs = new Mock<DbSet<Gig>>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);
            _repository = new GigRepository(mockContext.Object);
        }

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
    }
}
