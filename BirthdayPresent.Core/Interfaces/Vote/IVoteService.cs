namespace BirthdayPresent.Core.Interfaces.Vote
{
    using BirthdayPresent.Core.ViewModels.Vote;
    using System.Threading.Tasks;

    public interface IVoteService
    {
        Task<int> VoteForGiftAsync(int voteSessionId, int giftId, int voterId, CancellationToken cancellationToken);

        Task<VoteResultViewModel> GetVoteResultsAsync(int voteSessionId, int currentUserId, CancellationToken cancellationToken);
    }
}
