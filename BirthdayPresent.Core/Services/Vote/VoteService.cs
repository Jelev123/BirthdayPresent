namespace BirthdayPresent.Core.Services.Vote
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Enums;
    using BirthdayPresent.Core.Interfaces.Vote;
    using BirthdayPresent.Core.Services.Base;
    using BirthdayPresent.Core.ViewModels.Employee;
    using BirthdayPresent.Core.ViewModels.Vote;
    using BirthdayPresent.Core.ViewModels.VoteSession;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class VoteService : BaseService<Vote>, IVoteService
    {
        public VoteService(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<int> VoteForGiftAsync(int voteSessionId, int giftId, int voterId, CancellationToken cancellationToken)
        {
            var voteSession = await _data.VoteSessions
               .Where(vs => vs.Id == voteSessionId)
               .Select(vs => new VoteSessionViewModel
               {
                   Id = voteSessionId,
                   CreatedAt = vs.CreatedAt,
                   BirthdayEmployeeId = vs.BirthdayEmployeeId,
                   BirthdayEmployeeName = vs.BirthdayEmployee.FirstName + " " + vs.BirthdayEmployee.LastName,
                   StatusId = vs.StatusId,
               })
               .FirstOrDefaultAsync(cancellationToken);

            if (voteSession.StatusId == (int)VoteSessionStatusEnum.Closed)
            {
                throw new Exception(ErrorMessages.VoteSessionIsClosed);
            }

            ValidateVoteSessions(voteSession);
            ValidateBirthdayEmployee(voteSession, voterId);

            var voter = await _data.Users
                .Select(u => new EmployeeViewModel 
                {
                    Id = u.Id,
                    EmployeeName = u.FirstName + " " + u.LastName,
                })
                .FirstOrDefaultAsync(u => u.Id == voterId, cancellationToken);

            if (voter == null)
            {
                throw new Exception(ErrorMessages.VoterNotFound);
            }

            var existingVote = await _data.Votes
                .Select(v => new VoteViewModel 
                {
                    Id = v.Id,
                    GiftId = v.GiftId,
                    VoterId = v.VoterId,
                    VoteSessionId = v.VoteSessionId,
                })
                .FirstOrDefaultAsync(v => v.VoteSessionId == voteSessionId && v.VoterId == voterId, cancellationToken);

            if (existingVote != null)
            {
                throw new Exception(ErrorMessages.AlreadyVoted);
            }

            var gift = await _data.Gifts.FirstOrDefaultAsync(g => g.Id == giftId && g.IsActive, cancellationToken);

            if (gift == null)
            {
                throw new Exception(ErrorMessages.InvalidGift);
            }

            var vote = new Vote
            {
                VoterId = voterId,
                GiftId = giftId,
                VoteSessionId = voteSessionId,
                VotedAt = DateTime.UtcNow
            };

            await _data.Votes.AddAsync(vote, cancellationToken);
            await _data.SaveChangesAsync(cancellationToken);

            var updatedVoteCount = await _data.Votes
                .CountAsync(v => v.VoteSessionId == voteSessionId && v.GiftId == giftId, cancellationToken);

            return updatedVoteCount;
        }

        public async Task<VoteResultViewModel> GetVoteResultsAsync(int voteSessionId, int currentUserId, CancellationToken cancellationToken)
        {
            var voteSession = await _data.VoteSessions
                .Where(vs => vs.Id == voteSessionId)
                .Select(vs => new VoteSessionViewModel
                {
                    Id = vs.Id,
                    StartDate = vs.StartDate,
                    EndDate = vs.EndDate,
                    BirthdayEmployeeName = vs.BirthdayEmployee.FirstName + " " + vs.BirthdayEmployee.LastName,
                    BirthdayEmployeeId = vs.BirthdayEmployeeId,
                    Votes = vs.Votes.Select(v => new VoteViewModel
                    {
                        Id = v.Id,
                        VoterId = v.VoterId,
                        GiftId = v.GiftId,
                        GiftName = v.Gift.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            ValidateVoteSessions(voteSession);
            ValidateBirthdayEmployee(voteSession, currentUserId);

            var allEmployees = await _data.Users
                .Where(u => !u.Deleted && u.Id != voteSession.BirthdayEmployeeId)
                .ToListAsync(cancellationToken);

            var voters = voteSession.Votes.Select(v => v.VoterId).ToList();

            var nonVoters = allEmployees.Where(u => !voters.Contains(u.Id)).ToList();

            var resultViewModel = new VoteResultViewModel
            {
                VoteSessionId = voteSession.Id,
                BirthdayEmployeeName = voteSession.BirthdayEmployeeName,
                FinishDate = voteSession.EndDate,
                Voters = voteSession.Votes
                    .Select(v => new VoterViewModel
                    {
                        VoterName = allEmployees.FirstOrDefault(e => e.Id == v.VoterId)?.FirstName + " " + allEmployees.FirstOrDefault(e => e.Id == v.VoterId)?.LastName,
                        VotedGift = v.GiftName
                    }).ToList(),
                NonVoters = nonVoters.Select(nv => nv.FirstName + " " + nv.LastName).ToList(),
                Gifts = voteSession.Votes
                    .GroupBy(v => v.GiftName)
                    .Select(g => new GiftResultViewModel
                    {
                        GiftName = g.Key,
                        VoteCount = g.Count(),
                        Voters = g.Select(v => allEmployees.FirstOrDefault(
                            e => e.Id == v.VoterId)?.FirstName + " " + allEmployees.FirstOrDefault(
                            e => e.Id == v.VoterId)?.LastName).ToList()
                    }).ToList()
            };

            return resultViewModel;
        }

        private void ValidateVoteSessions(object voteSession)
        {
            if (voteSession == null)
            {
                throw new Exception(ErrorMessages.VoteSessionNotFound);
            }
        }

        private void ValidateBirthdayEmployee(VoteSessionViewModel voteSession, int currentUserId)
        {
            if (voteSession.BirthdayEmployeeId == currentUserId)
            {
                throw new Exception(ErrorMessages.BirthdayEmployeeRestrict);
            }
        }
    }
}
