namespace BirthdayPresent.Controllers.VoteSession
{
    using BirthdayPresent.Controllers.Base;
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class VoteSessionController : BaseController
    {
        private readonly IVoteSessionService voteSessionService;

        public VoteSessionController(IVoteSessionService voteSessionService)
        {
            this.voteSessionService = voteSessionService;
        }

        [Authorize]
        public async Task<IActionResult> CreateVoteSession(int birthdayEmployeeId)
        {
            await voteSessionService.CreateVoteSessionAsync(CurrentUserId, birthdayEmployeeId, CancellationToken.None);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> CloseVoteSession(int voteSessionId)
        {
            await voteSessionService.CloseVoteSessionAsync(CurrentUserId, voteSessionId, CancellationToken.None);
            return RedirectToAction("VoteResults", "Vote", new { voteSessionId = voteSessionId });
        }

        [Authorize]
        public async Task<IActionResult> DeleteVoteSession(int voteSessionId)
        {
            await voteSessionService.DeleteVoteSession(CurrentUserId, voteSessionId, CancellationToken.None);
            return RedirectToAction("AllClosedSessions");
        }

        [Authorize]
        public async Task<IActionResult> SessionDetails(int sessionId)
        {
            var session = await voteSessionService.GetSessionDetailsAsync(sessionId, CurrentUserId, CancellationToken.None);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [Authorize]
        public async Task<IActionResult> AllActiveSessions(CancellationToken cancellationToken)
        {
            var sessions = await voteSessionService.GetAllActiveSessionsAsync(CurrentUserId, cancellationToken);
            return View(sessions);
        }

        [Authorize]
        public async Task<IActionResult> AllClosedSessions(CancellationToken cancellationToken)
        {
            var sessions = await voteSessionService.GetAllClosedSessionsAsync(CurrentUserId, cancellationToken);
            return View(sessions);
        }
    }
}
