using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Test.Extensions;
using GigHub.Core.Repositories;
using FluentAssertions;
using System.Web.Http.Results;

namespace GigHub.Test.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _controller;
        public GigsControllerTests()
        {
            var mockRepository = new Mock<IGigRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.SetupGet(u => u.Gigs).Returns(mockRepository.Object);

            _controller = new GigsController(mockUnitOfWork.Object);

            _controller.MockCurrentUser("1", "user1@nathanc85.com");
        }
        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
