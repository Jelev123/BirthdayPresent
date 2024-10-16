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
            var currentDate = DateTime.UtcNow;
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
                    HasBirthday = u.DateOfBirth.Month == currentDate.Month && u.DateOfBirth.Day >= currentDate.Day,
                    HasActiveSessionForYear = activeBirthdayEmployeeIds.Contains(u.Id)
                })
                .ToListAsync(cancellationToken);

            return employees;
        }
    }
}
