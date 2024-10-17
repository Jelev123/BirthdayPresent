namespace BirthdayPresent.Core.Services.VoteSession
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Enums;
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using BirthdayPresent.Core.Services.Base;
    using BirthdayPresent.Core.ViewModels.Gift;
    using BirthdayPresent.Core.ViewModels.VoteSession;
    using Infrastructure.Data;
    using Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class VoteSessionService : BaseService<VoteSession>, IVoteSessionService
    {
        public VoteSessionService(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task CreateVoteSessionAsync(int initiatorId, int birthdayEmployeeId, CancellationToken _cancellationToken)
        {
            var birthdayEmployee = await FindIdByIdOrDefaultAsync<Employee>(birthdayEmployeeId, _cancellationToken);

            var currentYear = DateTime.UtcNow.Year;

            if (initiatorId == birthdayEmployeeId)
            {
                throw new Exception(ErrorMessages.InitiatorAndBdEmployeeCannotBeTheSame);
            }

            var existingActiveSession = await _data.VoteSessions
                .FirstOrDefaultAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId
                                           && vs.StatusId == (int)VoteSessionStatusEnum.Active,
                                           _cancellationToken);

            if (existingActiveSession != null)
            {
                throw new Exception(ErrorMessages.ActiveSessionExist);
            }

            var existingYearlySession = await _data.VoteSessions
                .FirstOrDefaultAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId
                                           && vs.VotingYear >= currentYear,
                                           _cancellationToken);

            if (existingYearlySession != null)
            {
                throw new Exception(ErrorMessages.SessionAlreadyCreated);
            }

            var activeStatus = await _data.SessionStatuses
                .FirstOrDefaultAsync(s => s.Id == (int)VoteSessionStatusEnum.Active, _cancellationToken);

            if (activeStatus == null)
            {
                throw new Exception(ErrorMessages.ActiveStatusNotFound);
            }

            var voteSession = new VoteSession
            {
                InitiatorId = initiatorId,
                StatusId = activeStatus.Id,
                Status = activeStatus,
                VotingYear = currentYear,
                StartDate = DateTime.UtcNow,
                EndDate = birthdayEmployee.DateOfBirth.Date,
                BirthdayEmployeeId = birthdayEmployeeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await CreateEntityAsync(voteSession, _cancellationToken);
        }


        public async Task CloseVoteSessionAsync(int initiatorId, int voteSessionId, CancellationToken _cancellationToken)
        {
            var voteSession = await GetEntityByIdAsync(voteSessionId, _cancellationToken);

            EnsureInitiator(voteSession.InitiatorId, initiatorId);

            voteSession.StatusId = (int)VoteSessionStatusEnum.Closed;
            voteSession.EndDate = DateTime.UtcNow;

            await SaveModificationAsync(voteSession, _cancellationToken);
        }


        public async Task DeleteVoteSession(int initiatorId, int voteSessionId, CancellationToken _cancellationToken)
        {
            var voteSession = await GetEntityByIdAsync(voteSessionId, _cancellationToken);

            EnsureInitiator(voteSession.InitiatorId, initiatorId);

            voteSession.Deleted = true;
            await SaveModificationAsync(voteSession, _cancellationToken);
        }


        public async Task<VoteSessionViewModel> GetSessionDetailsAsync(int sessionId, int currentUserId, CancellationToken cancellationToken)
        {
            var sessionSpecificVoteCounts = await _data.Votes
                 .Where(v => v.VoteSessionId == sessionId)
                 .GroupBy(v => v.GiftId)
                 .Select(group => new
                 {
                     GiftId = group.Key,
                     VoteCount = group.Count()
                 })
                 .ToDictionaryAsync(g => g.GiftId, g => g.VoteCount, cancellationToken);

            var details = await _data.VoteSessions.Where(s => s.Id == sessionId)
              .Select(s => new VoteSessionViewModel()
              {
                  Id = sessionId,
                  BirthdayEmployeeId = s.BirthdayEmployeeId,
                  BirthdayEmployeeName = s.BirthdayEmployee.FirstName + " " + s.BirthdayEmployee.LastName,
                  InitiatorId = s.InitiatorId,
                  InitiatorName = s.Initiator.FirstName + " " + s.Initiator.LastName,
                  CreatedAt = s.CreatedAt,
                  Status = s.Status.Status,
                  VotingYear = s.VotingYear,
                  StartDate = s.StartDate,
                  EndDate = s.EndDate,
                  UpdatedAt = s.UpdatedAt,
                  CurrentUserId = currentUserId,
                  SessionSpecificVoteCounts = sessionSpecificVoteCounts,
                  AllGifts = _data.Gifts.Where(g => g.IsActive)
                 .Select(g => new AllGiftsViewModel
                 {
                     Id = g.Id,
                     Name = g.Name,
                     Description = g.Description,
                     VoteCount = g.Votes.Count(v => v.VoteSessionId == sessionId)
                 }).ToList()
              }).FirstOrDefaultAsync();

            if (details.BirthdayEmployeeId == currentUserId)
            {
                throw new Exception(ErrorMessages.BirthdayEmployeeRestrict);
            }

            if (details != null)
            {
                var userVote = await GetUserVoteAsync(sessionId, currentUserId, cancellationToken);
                details.UserVotedGiftId = userVote;
            }

            return details;
        }

        public async Task<IEnumerable<AllSessionsViewModel>> GetAllActiveSessionsAsync(int currentUserId, CancellationToken cancellationToken)
        {
            return await _data.VoteSessions
                 .Where(s => s.StatusId == (int)VoteSessionStatusEnum.Active
                             && s.BirthdayEmployeeId != currentUserId)
                 .Select(s => new AllSessionsViewModel
                 {
                     Id = s.Id,
                     BirthdaysEmployeerName = s.BirthdayEmployee.FirstName + " " + s.BirthdayEmployee.LastName,
                     CreatedAt = s.CreatedAt,
                     UpdatedAt = s.UpdatedAt,
                     Status = s.Status.Status,
                 })
                 .ToListAsync();
        }

        public async Task<IEnumerable<AllSessionsViewModel>> GetAllClosedSessionsAsync(int currentUserId, CancellationToken cancellationToken)
        {
            return await _data.VoteSessions
                 .Where(s => s.StatusId == (int)VoteSessionStatusEnum.Closed
                             && s.BirthdayEmployeeId != currentUserId
                             && s.Deleted == false)
                 .Select(s => new AllSessionsViewModel
                 {
                     Id = s.Id,
                     InitiatorId = s.InitiatorId,
                     BirthdaysEmployeerName = s.BirthdayEmployee.FirstName + " " + s.BirthdayEmployee.LastName,
                     CreatedAt = s.CreatedAt,
                     FinishDate = s.EndDate,
                     UpdatedAt = s.UpdatedAt,
                     Status = s.Status.Status,
                 })
                 .ToListAsync();
        }

        public async Task<int?> GetUserVoteAsync(int voteSessionId, int userId, CancellationToken cancellationToken)
        {
            var userVote = await _data.Votes
                .Where(v => v.VoteSessionId == voteSessionId && v.VoterId == userId)
                .Select(v => v.GiftId)
                .FirstOrDefaultAsync(cancellationToken);

            return userVote;
        }

        private void EnsureInitiator(int sessionInitiatorId, int requestInitiatorId)
        {
            if (sessionInitiatorId != requestInitiatorId)
            {
                throw new Exception(ErrorMessages.OnlyInitiator);
            }
        }
    }
}
