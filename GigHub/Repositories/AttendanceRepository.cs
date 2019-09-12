﻿using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.Repositories
{
    public class AttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Attendance> GetFutureAttendances(string currentUser)
        {
            return _context.Attendances
                            .Where(a => a.AttendeeId == currentUser && a.Gig.DateTime > DateTime.Now)
                            .ToList();
        }

        public bool UserIsAttendingGig(int gigId, string userId)
        {
            return _context.Attendances
                    .Any(a => a.GigId == gigId && a.AttendeeId == userId);
        }
    }
}