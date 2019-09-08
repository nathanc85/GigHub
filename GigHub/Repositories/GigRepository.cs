using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GigHub.Repositories
{
    public class GigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
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
    }
}