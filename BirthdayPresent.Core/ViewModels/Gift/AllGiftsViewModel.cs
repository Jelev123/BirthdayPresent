namespace BirthdayPresent.Core.ViewModels.Gift
{
    public class AllGiftsViewModel
    {
        public int GiftId { get; set; }

        public string GiftName { get; set; }

        public int VoteCount { get; set; }

        public bool IsVoted { get; set; }
    }
}
