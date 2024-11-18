//namespace BirthdayPresent.Test
//{
//    using BirthdayPresent.Core.Constants;
//    using BirthdayPresent.Core.Services.VoteSession;
//    using BirthdayPresent.Core.Enums;
//    using BirthdayPresent.Infrastructure.Data;
//    using BirthdayPresent.Infrastructure.Data.Models;
//    using Microsoft.EntityFrameworkCore;

//    public class VoteSessionServiceTest
//    {

//        private ApplicationDbContext GetInMemoryDbContext()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(Guid.NewGuid().ToString())
//                .Options;

//            return new ApplicationDbContext(options);
//        }

//        [Fact]
//        public async Task CreateVoteSession_NoExistingSession_Success()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.SessionStatuses.Add(new SessionStatus
//            {
//                Id = (int)VoteSessionStatusEnum.Active,
//                Status = "Active"
//            });

//            dbContext.Users.Add(new Employee
//            {
//                Id = 1,
//                FirstName = "John",
//                LastName = "Doe",
//                DateOfBirth = new DateTime(1990, 5, 20),
//                Deleted = false
//            });

//            await dbContext.SaveChangesAsync();

//            int birthdayEmployeeId = 1;
//            int initiatorId = 2;

//            await voteSessionService.CreateVoteSessionAsync(initiatorId, birthdayEmployeeId, CancellationToken.None);

//            var createdSession = await dbContext.VoteSessions.FirstOrDefaultAsync(vs => vs.BirthdayEmployeeId == birthdayEmployeeId);
//            Assert.NotNull(createdSession); 
//            Assert.Equal(birthdayEmployeeId, createdSession.BirthdayEmployeeId);
//        }

//        [Fact]
//        public async Task CreateVoteSession_ExistingSessionForCurrentYear_ThrowsException()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.VoteSessions.Add(new VoteSession
//            {
//                Id = 1,
//                InitiatorId = 2,
//                BirthdayEmployeeId = 1,
//                VotingYear = DateTime.UtcNow.Year,
//                StatusId = 1,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            });
//            await dbContext.SaveChangesAsync();

//            int birthdayEmployeeId = 1;
//            int initiatorId = 2;

//            var exception = await Assert.ThrowsAsync<Exception>(() =>
//                voteSessionService.CreateVoteSessionAsync(initiatorId, birthdayEmployeeId, CancellationToken.None));

//            Assert.Equal(ErrorMessages.ActiveSessionExist, exception.Message);
//        }

//        [Fact]
//        public async Task CloseVoteSession_InitiatorCanCloseSession_Success()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.VoteSessions.Add(new VoteSession
//            {
//                Id = 1,
//                InitiatorId = 2,
//                BirthdayEmployeeId = 1,
//                VotingYear = DateTime.UtcNow.Year,
//                StatusId = (int)VoteSessionStatusEnum.Active,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            });
//            await dbContext.SaveChangesAsync();

//            int voteSessionId = 1;
//            int initiatorId = 2;

//            await voteSessionService.CloseVoteSessionAsync(initiatorId, voteSessionId, CancellationToken.None);

//            var closedSession = await dbContext.VoteSessions.FindAsync(voteSessionId);
//            Assert.NotNull(closedSession);
//            Assert.Equal((int)VoteSessionStatusEnum.Closed, closedSession.StatusId);
//            Assert.Equal(DateTime.UtcNow.Date, closedSession.EndDate.Value.Date);
//        }

//        [Fact]
//        public async Task CloseVoteSession_NonInitiatorCannotCloseSession_ThrowsException()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.VoteSessions.Add(new VoteSession
//            {
//                Id = 1,
//                InitiatorId = 2,
//                BirthdayEmployeeId = 1,
//                VotingYear = DateTime.UtcNow.Year,
//                StatusId = (int)VoteSessionStatusEnum.Active,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            });
//            await dbContext.SaveChangesAsync();

//            int voteSessionId = 1;
//            int nonInitiatorId = 3;

//            var exception = await Assert.ThrowsAsync<Exception>(() =>
//                voteSessionService.CloseVoteSessionAsync(nonInitiatorId, voteSessionId, CancellationToken.None));

//            Assert.Equal(ErrorMessages.OnlyInitiator, exception.Message);
//        }

//        [Fact]
//        public async Task DeleteVoteSession_InitiatorDeletesSession_Success()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.VoteSessions.Add(new VoteSession
//            {
//                Id = 1,
//                InitiatorId = 2,
//                BirthdayEmployeeId = 1,
//                VotingYear = DateTime.UtcNow.Year,
//                StatusId = (int)VoteSessionStatusEnum.Active,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            });
//            await dbContext.SaveChangesAsync();

//            int voteSessionId = 1;
//            int initiatorId = 2;

//            await voteSessionService.DeleteVoteSession(initiatorId, voteSessionId, CancellationToken.None);

//            var deletedSession = await dbContext.VoteSessions.FindAsync(voteSessionId);
//            Assert.NotNull(deletedSession);
//            Assert.True(deletedSession.Deleted);
//        }

//        [Fact]
//        public async Task DeleteVoteSession_NonInitiatorCannotDeleteSession_ThrowsException()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.VoteSessions.Add(new VoteSession
//            {
//                Id = 1,
//                InitiatorId = 2,
//                BirthdayEmployeeId = 1,
//                VotingYear = DateTime.UtcNow.Year,
//                StatusId = (int)VoteSessionStatusEnum.Active,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            });
//            await dbContext.SaveChangesAsync();

//            int voteSessionId = 1;
//            int nonInitiatorId = 3;

//            var exception = await Assert.ThrowsAsync<Exception>(() =>
//                voteSessionService.DeleteVoteSession(nonInitiatorId, voteSessionId, CancellationToken.None));

//            Assert.Equal(ErrorMessages.OnlyInitiator, exception.Message);
//        }

//        [Fact]
//        public async Task GetSessionDetailsAsync_BirthdayEmployee_ThrowsException()
//        {
//            var dbContext = GetInMemoryDbContext();
//            var voteSessionService = new VoteSessionService(dbContext);

//            dbContext.Users.Add(new Employee
//            {
//                Id = 1,
//                FirstName = "John",
//                LastName = "Doe",
//                DateOfBirth = new DateTime(1990, 5, 20)
//            });

//            dbContext.Users.Add(new Employee
//            {
//                Id = 2,
//                FirstName = "Jane",
//                LastName = "Doe",
//                DateOfBirth = new DateTime(1990, 7, 15)
//            });

//            dbContext.SessionStatuses.Add(new SessionStatus
//            {
//                Id = 1,
//                Status = "Active"
//            });


//            dbContext.VoteSessions.Add(new VoteSession
//            {
//                Id = 1,
//                InitiatorId = 2, 
//                BirthdayEmployeeId = 1,
//                VotingYear = DateTime.UtcNow.Year,
//                StatusId = 1,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            });

//            await dbContext.SaveChangesAsync();

//            int sessionId = 1;
//            int currentUserId = 1;

//            var exception = await Assert.ThrowsAsync<Exception>(() =>
//                voteSessionService.GetSessionDetailsAsync(sessionId, currentUserId, CancellationToken.None));

//            Assert.Equal(ErrorMessages.BirthdayEmployeeRestrict, exception.Message);
//        }

//    }
//}