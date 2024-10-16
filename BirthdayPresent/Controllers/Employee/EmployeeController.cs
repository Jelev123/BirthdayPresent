namespace BirthdayPresent.Controllers.Employee
{
    using BirthdayPresent.Controllers.Base;
    using BirthdayPresent.Core.Interfaces.Employee;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;
        
        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [Authorize]
        public async Task<IActionResult> AllEmployees(CancellationToken _cancellationToken)
        {
            var employees = await employeeService.GetAllAvailableAsync(_cancellationToken, CurrentUserId);
            return View(employees);
        }
    }
}
