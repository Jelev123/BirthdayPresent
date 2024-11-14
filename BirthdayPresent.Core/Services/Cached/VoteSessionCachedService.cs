namespace BirthdayPresent.Core.Services.Cached
{
    using BirthdayPresent.Core.Constants.VoteSessionCachedServiceParams;
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using BirthdayPresent.Core.Services.Employee;
    using BirthdayPresent.Core.Services.VoteSession;
    using BirthdayPresent.Core.ViewModels.VoteSession;
    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class VoteSessionCachedService : IVoteSessionService
    {
        private readonly VoteSessionService voteSessionService;
        private readonly EmployeeService employeeService;
        private readonly IMemoryCache _memoryCache;

        public VoteSessionCachedService(VoteSessionService voteSessionService, IMemoryCache memoryCache, EmployeeService employeeService)
        {
            this.voteSessionService = voteSessionService;
            _memoryCache = memoryCache;
            this.employeeService = employeeService;
        }

        public async Task CreateVoteSessionAsync(int initiatorId, int birthdayEmployeeId, CancellationToken _cancellationToken)
        {
            await voteSessionService.CreateVoteSessionAsync(initiatorId, birthdayEmployeeId, _cancellationToken);

            var cacheKey = string.Format(CachedServiceParams.CacheKeyActiveSession, initiatorId);
            _memoryCache.Remove(cacheKey);
        }

        public async Task CloseVoteSessionAsync(int initiatorId, int voteSessionId, CancellationToken _cancellationToken)
        {
            var cacheKeySessionDetails = string.Format(CachedServiceParams.CachedKeySessionDetails, voteSessionId, initiatorId);
            _memoryCache.Remove(cacheKeySessionDetails);

            var cacheKeyActiveSessions = string.Format(CachedServiceParams.CacheKeyActiveSession, initiatorId);
            _memoryCache.Remove(cacheKeyActiveSessions);

            var cacheKeyClosedSessions = string.Format(CachedServiceParams.CacheKeyClosedSession, initiatorId);
            _memoryCache.Remove(cacheKeyClosedSessions);

            await voteSessionService.CloseVoteSessionAsync(initiatorId, voteSessionId, _cancellationToken);
        }

        public async Task<VoteSessionViewModel> GetSessionDetailsAsync(int sessionId, int currentUserId, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CachedServiceParams.CachedKeySessionDetails, sessionId, currentUserId);

            if (!_memoryCache.TryGetValue(cacheKey, out VoteSessionViewModel sessionDetails))
            {
                sessionDetails = await voteSessionService.GetSessionDetailsAsync(sessionId, currentUserId, cancellationToken);

            var cacheOption = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CachedServiceParams._cacheDuration
            };

                _memoryCache.Set(cacheKey, sessionDetails, cacheOption);
            }

            return sessionDetails;
        }

        public async Task<IEnumerable<AllSessionsViewModel>> GetAllActiveSessionsAsync(int currentUserId, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CachedServiceParams.CacheKeyActiveSession, currentUserId);

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<AllSessionsViewModel> activeSessions))
            {
                activeSessions = await voteSessionService.GetAllActiveSessionsAsync(currentUserId, cancellationToken);

                var cacheOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CachedServiceParams._cacheDuration
                };

                _memoryCache.Set(cacheKey, activeSessions, cacheOption);
            }

            return activeSessions;
        }

        public async Task<IEnumerable<AllSessionsViewModel>> GetAllClosedSessionsAsync(int currentUserId, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CachedServiceParams.CacheKeyClosedSession, currentUserId);

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<AllSessionsViewModel> closedSessions))
            {
                closedSessions = await voteSessionService.GetAllClosedSessionsAsync(currentUserId, cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CachedServiceParams._cacheDuration
                };

                _memoryCache.Set(cacheKey, closedSessions, cacheOptions);
            }

            return closedSessions;
        }

        public async Task<int?> GetUserVoteAsync(int voteSessionId, int userId, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CachedServiceParams.CacheUserVote, voteSessionId, userId);

            if (!_memoryCache.TryGetValue(cacheKey, out int? userVote))
            {
                userVote = await employeeService.GetUserVoteAsync(voteSessionId, userId, cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CachedServiceParams._cacheDuration
                };

                _memoryCache.Set(cacheKey, userVote, cacheOptions);
            }

            return userVote;
        }

        public async Task DeleteVoteSession(int initiatorId, int voteSessionId, CancellationToken _cancellationToken)
        {
            var cacheKeySessionDetails = string.Format(CachedServiceParams.CachedKeySessionDetails, voteSessionId, initiatorId);
            var cacheKeyUserVote = string.Format(CachedServiceParams.CacheUserVote, voteSessionId, initiatorId);

            _memoryCache.Remove(cacheKeySessionDetails);
            _memoryCache.Remove(cacheKeyUserVote);

            var cacheKeyClosedSessions = string.Format(CachedServiceParams.CacheKeyClosedSession, initiatorId);
            _memoryCache.Remove(cacheKeyClosedSessions);

            await voteSessionService.DeleteVoteSession(initiatorId, voteSessionId, _cancellationToken);
        }
    }
}
