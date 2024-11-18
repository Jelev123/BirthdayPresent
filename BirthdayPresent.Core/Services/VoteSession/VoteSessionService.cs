namespace BirthdayPresent.Core.Services.VoteSession
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Enums;
    using BirthdayPresent.Core.Interfaces.Employee;
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
        private readonly IEmployeeService employeeService;
        private static string FormatFullName(string firstName, string lastName) => $"{firstName} {lastName}";

        public VoteSessionService(ApplicationDbContext dbContext, IEmployeeService employeeService) : base(dbContext)
        {
            this.employeeService = employeeService;
        }

        public async Task CreateVoteSessionAsync(int initiatorId, int birthdayEmployeeId, CancellationToken _cancellationToken)
        {
            ValidateInitiatorAndEmployee(initiatorId, birthdayEmployeeId);

            var birthdayEmployee = await FindEmployeeByIdOrThrowAsync(birthdayEmployeeId, _cancellationToken);

            await ValidateNoActiveOrYearlySessionAsync(birthdayEmployeeId, DateTime.UtcNow.Year, _cancellationToken);

            var activeStatus = await GetActiveStatusOrThrowAsync(_cancellationToken);

            var voteSession = CreateVoteSession(initiatorId, birthdayEmployeeId, birthdayEmployee, activeStatus);
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
            var details = await GetVoteSessionDetailsAsync(sessionId, currentUserId, cancellationToken);

            if (details == null)
            {
                throw new Exception(ErrorMessages.VoteSessionNotFound);
            }

            if (details.BirthdayEmployeeId == currentUserId)
            {
                throw new Exception(ErrorMessages.BirthdayEmployeeRestrict);
            }

            details.UserVotedGiftId = await employeeService.GetUserVoteAsync(sessionId, currentUserId, cancellationToken);

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

        private void EnsureInitiator(int sessionInitiatorId, int requestInitiatorId)
        {
            if (sessionInitiatorId != requestInitiatorId)
            {
                throw new Exception(ErrorMessages.OnlyInitiator);
            }
        }

        private void ValidateInitiatorAndEmployee(int initiatorId, int birthdayEmployeeId)
        {
            if (initiatorId == birthdayEmployeeId)
            {
                throw new Exception(ErrorMessages.InitiatorAndBdEmployeeCannotBeTheSame);
            }
        }

        private async Task<Employee> FindEmployeeByIdOrThrowAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await FindByIdOrDefaultAsync<Employee>(employeeId, cancellationToken);
            if (employee == null)
            {
                throw new Exception(ErrorMessages.VoterNotFound);
            }
            return employee;
        }

        private async Task ValidateNoActiveOrYearlySessionAsync(int birthdayEmployeeId, int currentYear, CancellationToken cancellationToken)
        {
            var sessionExists = await _data.VoteSessions
                .AnyAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId &&
                                (vs.StatusId == (int)VoteSessionStatusEnum.Active ||
                                 vs.VotingYear == currentYear),
                          cancellationToken);

            if (sessionExists)
            {
                throw new Exception(ErrorMessages.ActiveSessionExist);
            }
        }

        private async Task<SessionStatus> GetActiveStatusOrThrowAsync(CancellationToken cancellationToken)
        {
            var activeStatus = await _data.SessionStatuses
                .FirstOrDefaultAsync(s => s.Id == (int)VoteSessionStatusEnum.Active, cancellationToken);

            if (activeStatus == null)
            {
                throw new Exception(ErrorMessages.ActiveStatusNotFound);
            }

            return activeStatus;
        }

        private VoteSession CreateVoteSession(int initiatorId, int birthdayEmployeeId, Employee birthdayEmployee, SessionStatus activeStatus)
        {
            var currentUtcNow = DateTime.UtcNow;

            return new VoteSession
            {
                InitiatorId = initiatorId,
                StatusId = activeStatus.Id,
                Status = activeStatus,
                VotingYear = currentUtcNow.Year,
                StartDate = currentUtcNow,
                EndDate = birthdayEmployee.DateOfBirth.Date,
                BirthdayEmployeeId = birthdayEmployeeId,
                CreatedAt = currentUtcNow,
                UpdatedAt = currentUtcNow
            };
        }

        private async Task<VoteSessionViewModel> GetVoteSessionDetailsAsync(int sessionId, int currentUserId, CancellationToken cancellationToken)
        {
            var sessionSpecificVoteCounts = await GetSessionVoteCountsAsync(sessionId, cancellationToken);

            return await _data.VoteSessions
                .Where(s => s.Id == sessionId)
                .Select(s => new VoteSessionViewModel
                {
                    Id = s.Id,
                    BirthdayEmployeeId = s.BirthdayEmployeeId,
                    BirthdayEmployeeName = FormatFullName(s.BirthdayEmployee.FirstName, s.BirthdayEmployee.LastName),
                    InitiatorId = s.InitiatorId,
                    InitiatorName = FormatFullName(s.Initiator.FirstName, s.Initiator.LastName),
                    CreatedAt = s.CreatedAt,
                    Status = s.Status.Status,
                    VotingYear = s.VotingYear,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    UpdatedAt = s.UpdatedAt,
                    CurrentUserId = currentUserId,
                    SessionSpecificVoteCounts = sessionSpecificVoteCounts,
                    AllGifts = GetAllGiftsWithVoteCounts(sessionId)
                }).FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<Dictionary<int, int>> GetSessionVoteCountsAsync(int sessionId, CancellationToken cancellationToken)
        {
            return await _data.Votes
                .Where(v => v.VoteSessionId == sessionId)
                .GroupBy(v => v.GiftId)
                .Select(group => new { GiftId = group.Key, VoteCount = group.Count() })
                .ToDictionaryAsync(g => g.GiftId, g => g.VoteCount, cancellationToken);
        }

        private List<AllGiftsViewModel> GetAllGiftsWithVoteCounts(int sessionId)
        {
            return _data.Gifts
                .Where(g => g.IsActive)
                .Select(g => new AllGiftsViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    VoteCount = g.Votes.Count(v => v.VoteSessionId == sessionId)
                })
                .ToList();
        }
    }
}
