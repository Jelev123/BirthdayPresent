namespace BirthdayPresent.Core.ViewModels.Gift
{
    using BirthdayPresent.Core.ViewModels.Vote;

    public class GiftByIdViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<VoteViewModel> Votes { get; set; }
    }
}
