namespace BirthdayPresent.Core.Services.Employee
{
    using BirthdayPresent.Core.Interfaces.Employee;
    using BirthdayPresent.Core.Services.Base;
    using BirthdayPresent.Core.ViewModels.Employee;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        public EmployeeService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<AllEmployeesViewModel>> GetAllAvailableAsync(CancellationToken cancellationToken, int currentUserId)
        {
            var currentDate = DateTime.UtcNow.Date;
            var currentYear = currentDate.Year;

            var activeSessions = await _data.VoteSessions
                .Where(vs => vs.VotingYear == currentYear)
                .ToListAsync(cancellationToken);

            var activeBirthdayEmployeeIds = activeSessions.Select(vs => vs.BirthdayEmployeeId).ToList();

            var employees = await _data.Users
                .Where(u => !u.Deleted && u.Id != currentUserId)
                .Select(u => new AllEmployeesViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    HasBirthday = new DateTime(currentYear, u.DateOfBirth.Month, u.DateOfBirth.Day) >= currentDate,
                    HasActiveSessionForYear = activeBirthdayEmployeeIds.Contains(u.Id)
                })
                .ToListAsync(cancellationToken);

            return employees;
        }

        public async Task<int?> GetUserVoteAsync(int voteSessionId, int userId, CancellationToken cancellationToken)
        {
            var userVote = await _data.Votes
                .Where(v => v.VoteSessionId == voteSessionId && v.VoterId == userId)
                .Select(v => v.GiftId)
                .FirstOrDefaultAsync(cancellationToken);

            return userVote;
        }
    }
}
