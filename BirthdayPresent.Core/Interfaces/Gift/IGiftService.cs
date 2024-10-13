namespace BirthdayPresent.Core.Interfaces.Gift
{
    using BirthdayPresent.Core.ViewModels.Gift;

    public interface IGiftService
    {
        Task<IEnumerable<AllGiftsViewModel>> GetAllGiftsAsync();

        Task<GiftByIdViewModel> GiftById(int id);

        Task VoteAsync(int id, string userId);
    }
}
