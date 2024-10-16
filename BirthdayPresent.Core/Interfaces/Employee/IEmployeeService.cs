namespace BirthdayPresent.Core.Interfaces.Employee
{
    using BirthdayPresent.Core.ViewModels.Employee;

    public interface IEmployeeService
    {
        Task<IEnumerable<AllEmployeesViewModel>> GetAllAvailableAsync(CancellationToken _cancellationToken, int currentUserId);
    }
}
