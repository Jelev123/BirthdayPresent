namespace BirthdayPresent.Core.Services.Employee
{
    using BirthdayPresent.Core.Interfaces.Employee;
    using BirthdayPresent.Core.ViewModels.Employee;
    using BirthdayPresent.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext data;

        public EmployeeService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<AllEmployeesViewModel>> GetAllEmployeesAsync()
        {
            return await data.Users.Select(x => new AllEmployeesViewModel
            {
                Id = x.Id,
                Name = x.UserName,
                DateOfBirth = x.DateOfBirth,
                IsHasABirthDay = x.IsHasABirthDay,
     
            }).ToListAsync();
        }
    }
}
