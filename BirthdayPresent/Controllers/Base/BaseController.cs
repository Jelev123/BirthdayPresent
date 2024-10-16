namespace BirthdayPresent.Controllers.Base
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class BaseController : Controller
    {
        protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? throw new UnauthorizedAccessException());
    }
}
