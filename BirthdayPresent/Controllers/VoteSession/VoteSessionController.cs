namespace BirthdayPresent.Controllers.VoteSession
{
    using BirthdayPresent.Controllers.Base;
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class VoteSessionController : BaseController
    {
        private readonly IVoteSessionService _voteSessionService;

        public VoteSessionController(IVoteSessionService voteSessionService)
        {
            _voteSessionService = voteSessionService;
        }

        public async Task<IActionResult> StartVoteSession(int birthdayEmployeeId, CancellationToken cancellationToken)
        {
            try
            {
                await _voteSessionService.CreateVoteSessionAsync(CurrentUserId, birthdayEmployeeId, cancellationToken);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        public async Task<IActionResult> CloseVoteSession(int voteSessionId, CancellationToken cancellationToken)
        {
            try
            {
                await _voteSessionService.CloseVoteSessionAsync(CurrentUserId, voteSessionId, cancellationToken);
                return RedirectToAction("VoteResults", "Vote", new { VoteSessionId = voteSessionId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        public async Task<IActionResult> DeleteVoteSession(int voteSessionId, CancellationToken cancellationToken)
        {
            try
            {
                await _voteSessionService.DeleteVoteSession(CurrentUserId, voteSessionId, cancellationToken);
                return RedirectToAction("AllClosedSessions");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

        public async Task<IActionResult> SessionDetails(int sessionId, CancellationToken cancellationToken)
        {
            var session = await _voteSessionService.GetSessionDetailsAsync(sessionId, CurrentUserId, cancellationToken);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        public async Task<IActionResult> AllActiveSessions(CancellationToken cancellationToken)
        {
            var sessions = await _voteSessionService.GetAllActiveSessionsAsync(CurrentUserId, cancellationToken);
            return View(sessions);
        }

        public async Task<IActionResult> AllClosedSessions(CancellationToken cancellationToken)
        {
            var sessions = await _voteSessionService.GetAllClosedSessionsAsync(CurrentUserId, cancellationToken);
            return View(sessions);
        }
    }
}
