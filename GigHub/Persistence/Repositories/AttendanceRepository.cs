using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
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

        public Attendance GetAttendance(int gigId, string currentUser)
        {
            return _context.Attendances
                .SingleOrDefault(a => a.GigId == gigId && a.AttendeeId == currentUser);
        }

        public void Add(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
        }

        public void Remove(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
        }
    }
}