using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> GetFutureAttendances(string currentUser);
        Attendance GetAttendance(int gigId, string currentUser);
        void Add(Attendance attendance);
        void Remove(Attendance attendance);
    }
}