using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;

namespace MVC.Authorization
{
    public static class ContextParser
    {
        public static int GetUserId(HttpRequestContext context)
        {
            var principal = context.Principal as ClaimsPrincipal;

            var userId = int.Parse(principal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Single().Value);

            return userId;
        }
    }
}