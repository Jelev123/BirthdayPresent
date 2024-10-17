namespace BirthdayPresent.Core.Constants.VoteSessionCachedServiceParams
{
    public static class CachedServiceParams
    {
        public const string CachedKeyCloseSession = @"vote_session_{0}";

        public const string CachedKeySessionDetails = @"vote_session_{0}_details_{1}";

        public const string CacheKeyActiveSession = @"all_active_sessions_{0}";

        public const string CacheKeyClosedSession = @"all_closed_sessions_{0}";

        public const string CacheUserVote = @"user_vote_{0}_{1}";

        public const string CachedKeyVoteResult = @"vote_result_{0}_{1}";

        public static TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
    }
}
