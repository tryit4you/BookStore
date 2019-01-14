using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Route("admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index() => View(GetData(nameof(Index)));

        [Authorize(Roles = "Administrator")]
        public IActionResult OtherAction() => View("Index", GetData(nameof(OtherAction)));

        private Dictionary<string, object> GetData(string actionName) =>
 new Dictionary<string, object>
 {
     ["Action"] = actionName,
     ["User"] = HttpContext.User.Identity.Name,
     ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
     ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
     ["In Users Role"] = HttpContext.User.IsInRole("Users")
 };
    }
}