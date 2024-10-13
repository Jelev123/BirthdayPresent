namespace BirthdayPresent.Core.Services.VoteSession
{
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using BirthdayPresent.Core.ViewModels.VoteSession;
    using BirthdayPresent.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class VoteSessionService : IVoteSessionService
    {
        private readonly ApplicationDbContext data;

        public VoteSessionService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<VoteSessionViewModel> GetSessionDetailsAsync(int sessionId)
        {
            return await data.VoteSessions.Where(s => s.Id == sessionId)
                .Select(s => new VoteSessionViewModel()
                {
                    BirthdayEmployeeId = s.BirthdayEmployeeId,
                    BirthdayEmployeeName = s.BirthdayEmployee.FirstName + " " + s.BirthdayEmployee.LastName,
                    InitiatorId = s.InitiatorId,
                    InitiatorName = s.Initiator.FirstName + " " + s.Initiator.LastName,
                    CreatedAt = s.CreatedAt,
                    Status = s.Status,
                    VotingYear = s.VotingYear,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    UpdatedAt = s.UpdatedAt,
                }).FirstOrDefaultAsync();
        }

        public async Task<VoteSessionViewModel> StartSession(string initiatorId, string birthdayEmployeeId)
        {
            // Step 1: Get the birthday employee's upcoming birthday date
            var birthdayEmployee = await data.Users.FirstOrDefaultAsync(u => u.Id == birthdayEmployeeId);
            if (birthdayEmployee == null)
            {
                throw new Exception("Birthday employee not found.");
            }

            // Step 2: Calculate the next birthday (or you can choose any other DateTime as the VotingYear)
            var upcomingBirthday = GetNextBirthday(birthdayEmployee.DateOfBirth);

            // Step 3: Check if there is already an active voting session for the same birthday employee
            bool isActiveSessionExists = await data.VoteSessions
                .AnyAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId && vs.Status == "Active");

            if (isActiveSessionExists)
            {
                throw new InvalidOperationException("An active voting session already exists for this birthday employee.");
            }

            // Step 4: Check if there is a voting session for the same birthday year
            bool isSessionForSameYearExists = await data.VoteSessions
                .AnyAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId && vs.VotingYear.Year == upcomingBirthday.Year);

            if (isSessionForSameYearExists)
            {
                throw new InvalidOperationException("A voting session already exists for this birthday employee for the current year.");
            }

            // Step 5: Create a new voting session
            var newSession = new Infrastructure.Data.Models.VoteSession
            {
                InitiatorId = initiatorId,
                BirthdayEmployeeId = birthdayEmployeeId,
                StartDate = DateTime.UtcNow,
                VotingYear = upcomingBirthday, // VotingYear is now the upcoming birthday
                Status = "Active"
            };

            // Step 6: Add the new session to the database and save changes
            await data.VoteSessions.AddAsync(newSession);
            await data.SaveChangesAsync();

            // Step 7: Map to VoteSessionViewModel (or you can return the VoteSession entity if preferred)
            var sessionViewModel = new VoteSessionViewModel
            {
                Id = newSession.Id,
                InitiatorId = newSession.InitiatorId,
                BirthdayEmployeeId = newSession.BirthdayEmployeeId,
                StartDate = newSession.StartDate,
                VotingYear = newSession.VotingYear,
                Status = newSession.Status
            };

            return sessionViewModel;
        }

        private DateTime GetNextBirthday(DateTime dateOfBirth)
        {
            var now = DateTime.UtcNow;
            var nextBirthday = new DateTime(now.Year, dateOfBirth.Month, dateOfBirth.Day);

            // If the next birthday in the current year has already passed, set it to next year
            if (nextBirthday < now)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }

            return nextBirthday;
        }
    }
}
