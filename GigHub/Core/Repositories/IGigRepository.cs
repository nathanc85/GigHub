using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        void Add(Gig gig);
        IEnumerable<Gig> GetFutureGigsWithGenre(string userId);
        Gig GetGig(int gigUd);
        IEnumerable<Gig> GetGigsUserAttending(string currentUser);
        Gig GetGigWithArtistAndGenre(int gigId);
        Gig GetGigWithAttendees(int gigId);
    }
}