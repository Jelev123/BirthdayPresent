namespace BirthdayPresent.Core.Interfaces.Gift
{
    using BirthdayPresent.Core.ViewModels.Gift;

    public interface IGiftService
    {
        Task<IEnumerable<AllGiftsViewModel>> GetAllGiftsAsync(CancellationToken cancellationToken);
    }
}
