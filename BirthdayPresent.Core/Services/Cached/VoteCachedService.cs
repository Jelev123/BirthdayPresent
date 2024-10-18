namespace BirthdayPresent.Core.Services.Cached
{
    using BirthdayPresent.Core.Constants.VoteSessionCachedServiceParams;
    using BirthdayPresent.Core.Interfaces.Vote;
    using BirthdayPresent.Core.Services.Vote;
    using BirthdayPresent.Core.ViewModels.Vote;
    using Microsoft.Extensions.Caching.Memory;
    using System.Threading;
    using System.Threading.Tasks;

    public class VoteCachedService : IVoteService
    {
        private readonly VoteService voteService;
        private readonly IMemoryCache _memoryCache;

        public VoteCachedService(VoteService voteService, IMemoryCache memoryCache)
        {
            this.voteService = voteService;
            _memoryCache = memoryCache;
        }

        public async Task<VoteResultViewModel> GetVoteResultsAsync(int voteSessionId, int currentUserId, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CachedServiceParams.CachedKeyVoteResult, voteSessionId, currentUserId);

            if (!_memoryCache.TryGetValue(cacheKey, out VoteResultViewModel voteResult))
            {
                voteResult = await voteService.GetVoteResultsAsync(voteSessionId, currentUserId, cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CachedServiceParams._cacheDuration
                };
                _memoryCache.Set(cacheKey, voteResult, cacheOptions);
            }

            return voteResult;
        }

        public async Task<int> VoteForGiftAsync(int voteSessionId, int giftId, int voterId, CancellationToken cancellationToken)
        {
            var cacheKeyVoteResult = string.Format(CachedServiceParams.CachedKeyVoteResult, voteSessionId, "*");
            _memoryCache.Remove(cacheKeyVoteResult);

            var cacheKeySessionDetails = string.Format(CachedServiceParams.CachedKeySessionDetails, voteSessionId, voterId);
            _memoryCache.Remove(cacheKeySessionDetails);

            return await voteService.VoteForGiftAsync(voteSessionId, giftId, voterId, cancellationToken);
        }
    }
}
