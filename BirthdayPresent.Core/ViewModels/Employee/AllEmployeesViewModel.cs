namespace BirthdayPresent.Core.ViewModels.Employee
{
    public class AllEmployeesViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool HasBirthday { get; set; }

        public bool HasActiveGlobalSession { get; set; }

        public bool HasActiveSessionForYear { get; set; }
    }
}
