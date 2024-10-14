namespace BirthdayPresent.Core.Services.VoteSession
{
    using BirthdayPresent.Core.Enums;
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using BirthdayPresent.Core.Services.Base;
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

        public async Task<VoteSessionViewModel> CreateVoteSessionAsync(int initiatorId, int birthdayEmployeeId, CancellationToken _cancellationToken)
        {
            if (initiatorId == birthdayEmployeeId)
            {
                throw new Exception("Initiator and birthday employee cannot be the same person");
            }

            // Check if there's already an active session for this employee
            var existingActiveSession = await _data.VoteSessions
                .FirstOrDefaultAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId
                                           && vs.StatusId == (int)VoteSessionStatusEnum.Active,
                                           _cancellationToken);

            if (existingActiveSession != null)
            {
                throw new Exception("An active session already exists for this employee.");
            }

            // Check if a session has already been created for this employee in the current year
            var existingYearlySession = await _data.VoteSessions
                .FirstOrDefaultAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId
                                           && vs.VotingYear == DateTime.UtcNow.Year,
                                           _cancellationToken);

            if (existingYearlySession != null)
            {
                throw new Exception("A session has already been created for this employee this year.");
            }

            var birthdayEmployee = await this.FindIdByIdOrDefaultAsync<Employee>(birthdayEmployeeId, _cancellationToken);

            var activeStatus = await _data.SessionStatuses
                .FirstOrDefaultAsync(s => s.Id == (int)VoteSessionStatusEnum.Active, _cancellationToken);

            if (activeStatus == null)
            {
                throw new Exception("The active status could not be found in the database.");
            }

            var voteSession = new VoteSession
            {
                InitiatorId = initiatorId,
                StatusId = activeStatus.Id,
                Status = activeStatus,
                VotingYear = DateTime.UtcNow.Year,
                StartDate = DateTime.UtcNow,
                EndDate = birthdayEmployee.DateOfBirth.Date,
                BirthdayEmployeeId = birthdayEmployeeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await this.CreateEntityAsync(voteSession, _cancellationToken);

            var sessionViewModel = new VoteSessionViewModel
            {
                Id = voteSession.Id,
                InitiatorId = voteSession.InitiatorId,
                BirthdayEmployeeId = voteSession.BirthdayEmployeeId,
                StartDate = voteSession.StartDate,
                VotingYear = voteSession.VotingYear,
                Status = voteSession.Status.Status
            };

            return sessionViewModel;
        }


        public async Task CloseVoteSessionAsync(int initiatorId, int voteSessionId, CancellationToken _cancellationToken)
        {
            var voteSession = await this.GetEntityByIdAsync(voteSessionId, _cancellationToken);

            if (voteSession.InitiatorId != initiatorId)
            {
                throw new Exception("Only the initiator can close the vote session");
            }

            voteSession.StatusId = (int)VoteSessionStatusEnum.Closed;

            await this.SaveModificationAsync(voteSession, _cancellationToken);
        }

        public async Task<VoteSessionViewModel> GetSessionDetailsAsync(int sessionId, int currentUserId)
        {
            return await _data.VoteSessions.Where(s => s.Id == sessionId)
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
                 }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AllSessionsViewModel>> GetAllActiveSessionsAsync(int currentUserId)
        {
            return await _data.VoteSessions
                 .Where(s => s.StatusId == (int)VoteSessionStatusEnum.Active
                             && s.BirthdayEmployeeId != currentUserId)
                 .Select(s => new AllSessionsViewModel
                 {
                     Id = s.Id,
                     BirthdaysEmployeerName = s.BirthdayEmployee.FirstName + " " + s.BirthdayEmployee.LastName,
                     CreatedAt = s.CreatedAt,
                     FinishDate = s.EndDate,
                     UpdatedAt = s.UpdatedAt
                 })
                 .ToListAsync();
        }

        public async Task<IEnumerable<AllSessionsViewModel>> GetAllClosedSessionsAsync(int currentUserId)
        {
            return await _data.VoteSessions
                 .Where(s => s.StatusId == (int)VoteSessionStatusEnum.Active
                             && s.BirthdayEmployeeId != currentUserId)
                 .Select(s => new AllSessionsViewModel
                 {
                     Id = s.Id,
                     BirthdaysEmployeerName = s.BirthdayEmployee.FirstName + " " + s.BirthdayEmployee.LastName,
                     CreatedAt = s.CreatedAt,
                     FinishDate = s.EndDate,
                     UpdatedAt = s.UpdatedAt
                 })
                 .ToListAsync();
        }
    }
}
