namespace BirthdayPresent.Controllers.Gift
{
    using BirthdayPresent.Core.Interfaces.Gift;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class GiftController : Controller
    {
        private readonly IGiftService giftService;

        private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? throw new UnauthorizedAccessException();

        public GiftController(IGiftService giftService)
        {
            this.giftService = giftService;
        }

        public async Task<IActionResult> AllGifts()
        {
            return View(await this.giftService.GetAllGiftsAsync());
        }


        public async Task<IActionResult> GiftById(int id)
        {
           return View(await giftService.GiftById(id));
        }


        public async Task<IActionResult> Vote(int id)
        {
            await giftService.VoteAsync(id, CurrentUserId);
            return RedirectToAction("AllGifts");
        }
    }
}
