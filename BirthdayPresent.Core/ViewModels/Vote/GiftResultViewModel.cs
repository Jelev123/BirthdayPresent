namespace BirthdayPresent.Core.ViewModels.Vote
{
    using System.Collections.Generic;

    public class GiftResultViewModel
    {
        public int Id { get; set; }

        public string GiftName { get; set; }

        public int VoteCount { get; set; }

        public bool IsActive { get; set; }

        public List<string> Voters { get; set; }
    }
}
