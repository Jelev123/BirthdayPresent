namespace BirthdayPresent.Controllers.Vote
{
    using BirthdayPresent.Controllers.Base;
    using BirthdayPresent.Core.Constants;
    using BirthdayPresent.Core.Interfaces.Vote;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class VoteController : BaseController
    {
        private readonly IVoteService voteService;

        public VoteController(IVoteService voteService)
        {
            this.voteService = voteService;
        }

        [Authorize]
        public async Task<IActionResult> SubmitVote(int voteSessionId, int giftId)
        {
            try
            {
                var updatedVoteCount = await voteService.VoteForGiftAsync(voteSessionId, giftId, CurrentUserId, CancellationToken.None);

                return Json(new { success = true, message = InformationMessages.SeccessVote, updatedVoteCount });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        public async Task<IActionResult> VoteResults(int voteSessionId)
        {
            try
            {
                var voteResults = await voteService.GetVoteResultsAsync(voteSessionId, CurrentUserId, CancellationToken.None);
                return View(voteResults);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
