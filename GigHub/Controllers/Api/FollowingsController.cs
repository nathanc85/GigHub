using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        ApplicationDbContext _context;
        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var currentUser = User.Identity.GetUserId();
            var exists = _context.Followings.Any(f => f.FolloweeId == dto.FolloweeId && f.FollowerId == currentUser);

            if (exists)
            {
                return BadRequest("You already follow this artist.");
            }

            Following newFollowing = new Following()
            {
                FollowerId = currentUser,
                FolloweeId = dto.FolloweeId
            };

            _context.Followings.Add(newFollowing);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string id) {
            var currentUser = User.Identity.GetUserId();

            var following = _context.Followings
                .SingleOrDefault(f => f.FolloweeId == id && f.FollowerId == currentUser);

            if (following == null)
            {
                return NotFound();
            }

            _context.Followings.Remove(following);
            _context.SaveChanges();

            return Ok(id);
        }
    }
}
