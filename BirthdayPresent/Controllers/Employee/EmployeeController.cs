namespace BirthdayPresent.Controllers.Employee
{
    using BirthdayPresent.Core.Interfaces.Employee;
    using Microsoft.AspNetCore.Mvc;

    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> AllEmployees()
        {
            return View(await this.employeeService.GetAllEmployeesAsync());
        }
    }
}
