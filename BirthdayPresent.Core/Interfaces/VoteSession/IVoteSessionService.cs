namespace BirthdayPresent.Core.Interfaces.VoteSession
{
    using BirthdayPresent.Core.ViewModels.VoteSession;

    public interface IVoteSessionService
    {
        Task<VoteSessionViewModel> StartSession(string initiatorId, string birthdayEmployeeId);

        Task<VoteSessionViewModel> GetSessionDetailsAsync(int sessionId);
    }
}
