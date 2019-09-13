using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repositories
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> GetFutureAttendances(string currentUser);
        bool UserIsAttendingGig(int gigId, string userId);
    }
}