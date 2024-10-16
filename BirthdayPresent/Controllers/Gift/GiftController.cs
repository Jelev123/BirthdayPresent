namespace BirthdayPresent.Controllers.Gift
{
    using BirthdayPresent.Core.Interfaces.Gift;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class GiftController : Controller
    {
        private readonly IGiftService giftService;

        public GiftController(IGiftService giftService)
        {
            this.giftService = giftService;
        }

        [Authorize]
        public async Task<IActionResult> AllGifts(CancellationToken cancellationToken)
        {
            return View(await giftService.GetAllGiftsAsync(cancellationToken));
        } 
    }
}
