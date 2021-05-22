using GScript.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GScript.Filters
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute,
        IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if ( !user.Identity.IsAuthenticated )
            {
                context.Result = ToolkitService.GetRoute("Login", "Identity");
            }
            else if ( !user.IsInRole("admin") )
            {
                context.Result = ToolkitService.GetRoute("AccessDenied", "Identity");
            }
        }
    }
}
