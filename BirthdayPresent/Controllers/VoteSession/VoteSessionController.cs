namespace BirthdayPresent.Controllers.VoteSession
{
    using BirthdayPresent.Core.Interfaces.VoteSession;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class VoteSessionController : Controller
    {
        private readonly IVoteSessionService voteSessionService;
        private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? throw new UnauthorizedAccessException();

        public VoteSessionController(IVoteSessionService voteSessionService)
        {
            this.voteSessionService = voteSessionService;
        }

        public async Task<IActionResult> StartSession(string birthdayEmployeeId)
        {
            var initiatorId = CurrentUserId;

            birthdayEmployeeId = "3c8fc12a-2f51-4bc3-a48a-69dd08a90722";
            try
            {
                var session = await voteSessionService.StartSession(initiatorId, birthdayEmployeeId);

                // Redirect to a view displaying the newly created session details
                return RedirectToAction("SessionDetails", new { sessionId = session.Id });
            }
            catch (InvalidOperationException ex)
            {
                // Handle any errors related to business logic (e.g., active session already exists)
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                TempData["ErrorMessage"] = "An error occurred while starting the voting session.";
                return RedirectToAction("Error");
            }
        }

        // Action to Show Details of an Existing Vote Session
        public async Task<IActionResult> SessionDetails(int sessionId)
        {
            var session = await voteSessionService.GetSessionDetailsAsync(sessionId);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }
    }
}
