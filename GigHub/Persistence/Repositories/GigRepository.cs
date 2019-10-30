using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GigHub.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly IApplicationDbContext _context;

        public GigRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Gig GetGig(int gigUd)
        {
            return _context.Gigs.SingleOrDefault(g => g.Id == gigUd);
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string currentUser)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == currentUser)
                .Select(g => g.Gig)
                .Include(a => a.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig GetGigWithArtistAndGenre(int gigId)
        {
            return _context.Gigs
                .Include(a => a.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetFutureGigsWithGenre(string userId)
        {
            return _context.Gigs
                .Where(g => g.ArtistId == userId
                    && g.DateTime >= DateTime.Now
                    && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
        }

        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}