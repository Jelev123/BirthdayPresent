namespace BirthdayPresent.Core.Interfaces.VoteSession
{
    using BirthdayPresent.Core.ViewModels.VoteSession;

    public interface IVoteSessionService
    {
        Task CreateVoteSessionAsync(int initiatorId, int birthdayEmployeeId, CancellationToken _cancellationToken);

        Task CloseVoteSessionAsync(int initiatorId, int voteSessionId, CancellationToken _cancellationToken);

        Task DeleteVoteSession(int initiatorId, int voteSessionId, CancellationToken _cancellationToken);

        Task<VoteSessionViewModel> GetSessionDetailsAsync(int sessionId, int currentUserId, CancellationToken cancellationToken);

        Task<IEnumerable<AllSessionsViewModel>> GetAllActiveSessionsAsync(int currentUserId, CancellationToken cancellationToken);

        Task<IEnumerable<AllSessionsViewModel>> GetAllClosedSessionsAsync(int currentUserId, CancellationToken cancellationToken);
    }
}
