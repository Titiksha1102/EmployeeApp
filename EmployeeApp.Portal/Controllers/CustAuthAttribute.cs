using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Security.Claims;

namespace EmployeeApp.Portal.Controllers
{
    public class CustAuthAttribute: ActionFilterAttribute
    {
        private readonly List<string> _adminUsers;

        public CustAuthAttribute(List<string> adminUsers)
        {
            _adminUsers = adminUsers;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Landing", "Employees", null);
                return;
            }

            var userName = user.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName) || !_adminUsers.Contains(userName))
            {
                context.Result = new RedirectToActionResult("Landing", "Employees", null);
                return;
            }
            else
            {
                context.HttpContext.Session.SetString("sessionUser", context.HttpContext.User.Identity.Name);
            }
            base.OnActionExecuting(context);
        }
    }
}

