namespace BirthdayPresent.Test
{
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Enums;
    using BirthdayPresent.Core.Services.Vote;
    using BirthdayPresent.Infrastructure.Data;
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class VoteServiceTest
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }


        [Fact]
        public async Task VoteForGiftAsync_VoteSessionClosed_ThrowsException()
        {
            var dbContext = GetInMemoryDbContext();
            var voteService = new VoteService(dbContext);

            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 20)
            };

            dbContext.Users.Add(employee);

            dbContext.VoteSessions.Add(new VoteSession
            {
                Id = 1,
                StatusId = (int)VoteSessionStatusEnum.Closed,
                BirthdayEmployeeId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BirthdayEmployee = employee
            });

            await dbContext.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                voteService.VoteForGiftAsync(1, 1, 2, CancellationToken.None));

            Assert.Equal(ErrorMessages.VoteSessionIsClosed, exception.Message);
        }


        [Fact]
        public async Task VoteForGiftAsync_AlreadyVoted_ThrowsException()
        {
            var dbContext = GetInMemoryDbContext();
            var voteService = new VoteService(dbContext);

            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 20)
            };

            var voter = new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 7, 15)
            };

            dbContext.Users.Add(employee);
            dbContext.Users.Add(voter);

            dbContext.VoteSessions.Add(new VoteSession
            {
                Id = 1,
                StatusId = (int)VoteSessionStatusEnum.Active,
                BirthdayEmployeeId = employee.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BirthdayEmployee = employee // Reuse the employee
            });

            dbContext.Votes.Add(new Vote
            {
                Id = 1,
                VoteSessionId = 1,
                GiftId = 1,
                VoterId = voter.Id
            });

            await dbContext.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                voteService.VoteForGiftAsync(1, 1, voter.Id, CancellationToken.None));

            Assert.Equal(ErrorMessages.AlreadyVoted, exception.Message);
        }


        [Fact]
        public async Task VoteForGiftAsync_InvalidGift_ThrowsException()
        {
            var dbContext = GetInMemoryDbContext();
            var voteService = new VoteService(dbContext);

            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 20)
            };

            var voter = new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 7, 15)
            };

            dbContext.Users.Add(employee);
            dbContext.Users.Add(voter);

            dbContext.VoteSessions.Add(new VoteSession
            {
                Id = 1,
                StatusId = (int)VoteSessionStatusEnum.Active,
                BirthdayEmployeeId = employee.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BirthdayEmployee = employee
            });

            await dbContext.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                voteService.VoteForGiftAsync(1, 999, voter.Id, CancellationToken.None));

            Assert.Equal(ErrorMessages.InvalidGift, exception.Message);
        }


        [Fact]
        public async Task GetVoteResultsAsync_BirthdayEmployeeRestrict_ThrowsException()
        {
            var dbContext = GetInMemoryDbContext();
            var voteService = new VoteService(dbContext);

            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 20)
            };

            dbContext.Users.Add(employee);

            dbContext.VoteSessions.Add(new VoteSession
            {
                Id = 1,
                StatusId = (int)VoteSessionStatusEnum.Active,
                BirthdayEmployeeId = employee.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BirthdayEmployee = employee
            });

            await dbContext.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                voteService.GetVoteResultsAsync(1, employee.Id, CancellationToken.None));

            Assert.Equal(ErrorMessages.BirthdayEmployeeRestrict, exception.Message);
        }
    }
}
