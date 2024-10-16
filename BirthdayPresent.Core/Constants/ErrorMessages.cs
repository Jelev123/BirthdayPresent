namespace BirthdayPresent.Core.Constants
{
    public static class ErrorMessages
    {
        public const string EntityDoesNotExist = @"{0} with {1} '{2}' does not exists!";

        public const string EntityAlreadyExists = @"{0} - '{1}' already exists!";

        public const string VoteSessionNotFound = "Vote session not found.";

        public const string BirthdayEmployeeRestrict = "Birth day employee cannot do this";

        public const string VoterNotFound = "VoterNotFound";

        public const string AlreadyVoted = "You have already voted in this session.";

        public const string InvalidGift = "Invalid or inactive gift.";

        public const string InitiatorAndBdEmployeeCannotBeTheSame = "Initiator and birthday employee cannot be the same person";

        public const string ActiveSessionExist = "An active session already exists for this employee.";

        public const string SessionAlreadyCreated = "A session has already been created for this employee this year.";

        public const string ActiveStatusNotFound = "The active status could not be found in the database.";

        public const string OnlyInitiator = "Only the initiator can close the vote session.";

        public const string BdayIsOver = "Birth day is over";
    }
}
