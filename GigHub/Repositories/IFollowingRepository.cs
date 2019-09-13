namespace GigHub.Repositories
{
    public interface IFollowingRepository
    {
        bool UserFollowingArtist(string artistId, string userId);
    }
}