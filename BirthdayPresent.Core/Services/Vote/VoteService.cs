namespace BirthdayPresent.Core.Services.Vote
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Enums;
    using BirthdayPresent.Core.Interfaces.Vote;
    using BirthdayPresent.Core.Services.Base;
    using BirthdayPresent.Core.ViewModels.Vote;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class VoteService : BaseService<Vote>, IVoteService
    {
        public VoteService(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<int> VoteForGiftAsync(int voteSessionId, int giftId, int voterId, CancellationToken cancellationToken)
        {
            var voteSession = await _data.VoteSessions
                .Include(vs => vs.BirthdayEmployee)
                .Where(vs => vs.Id == voteSessionId)
                .Select(vs => new { vs.Id, vs.StatusId, vs.BirthdayEmployeeId })
                .FirstOrDefaultAsync(cancellationToken);

            if (voteSession == null || voteSession.StatusId == (int)VoteSessionStatusEnum.Closed)
                throw new InvalidOperationException(ErrorMessages.VoteSessionIsClosed);

            if (voteSession.BirthdayEmployeeId == voterId)
                throw new InvalidOperationException(ErrorMessages.BirthdayEmployeeRestrict);

            var existingVote = await _data.Votes
                .AnyAsync(v => v.VoteSessionId == voteSessionId && v.VoterId == voterId, cancellationToken);

            if (existingVote)
                throw new InvalidOperationException(ErrorMessages.AlreadyVoted);

            var gift = await _data.Gifts
                .Where(g => g.Id == giftId && g.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (gift == null)
                throw new KeyNotFoundException(ErrorMessages.InvalidGift);

            var vote = new Vote
            {
                VoterId = voterId,
                GiftId = giftId,
                VoteSessionId = voteSessionId,
                VotedAt = DateTime.UtcNow
            };

            await _data.Votes.AddAsync(vote, cancellationToken);
            await _data.SaveChangesAsync(cancellationToken);

            return await _data.Votes
                .CountAsync(v => v.VoteSessionId == voteSessionId && v.GiftId == giftId, cancellationToken);
        }

        public async Task<VoteResultViewModel> GetVoteResultsAsync(int voteSessionId, int currentUserId, CancellationToken cancellationToken)
        {
            var voteSession = await _data.VoteSessions
                .Include(vs => vs.BirthdayEmployee)
                .Include(vs => vs.Votes)
                .ThenInclude(v => v.Gift)
                .Where(vs => vs.Id == voteSessionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (voteSession == null)
                throw new KeyNotFoundException(ErrorMessages.VoteSessionNotFound);

            if (voteSession.BirthdayEmployeeId == currentUserId)
                throw new InvalidOperationException(ErrorMessages.BirthdayEmployeeRestrict);

            var allEmployees = await _data.Users
                .Where(u => !u.Deleted && u.Id != voteSession.BirthdayEmployeeId)
                .ToListAsync(cancellationToken);

            var employeeLookup = allEmployees.ToDictionary(e => e.Id, e => e.FirstName + " " + e.LastName);

            var voters = voteSession.Votes.Select(v => v.VoterId).ToList();
            var nonVoters = allEmployees
                .Where(u => !voters.Contains(u.Id))
                .Select(nv => nv.FirstName + " " + nv.LastName)
                .ToList();

            var resultViewModel = new VoteResultViewModel
            {
                VoteSessionId = voteSession.Id,
                BirthdayEmployeeName = voteSession.BirthdayEmployee.FirstName + " " + voteSession.BirthdayEmployee.LastName,
                FinishDate = voteSession.EndDate,
                Voters = voteSession.Votes
                    .Select(v => new VoterViewModel
                    {
                        VoterName = employeeLookup[v.VoterId],
                        VotedGift = v.Gift.Name
                    }).ToList(),
                NonVoters = nonVoters,
                Gifts = voteSession.Votes
                    .GroupBy(v => v.Gift.Name)
                    .Select(g => new GiftResultViewModel
                    {
                        GiftName = g.Key,
                        VoteCount = g.Count(),
                        Voters = g.Select(v => employeeLookup[v.VoterId]).ToList()
                    }).ToList()
            };

            return resultViewModel;
        }
    }
}
