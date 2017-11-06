using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading;
using MVC.App_Start;
using Microsoft.AspNet.SignalR.Owin;

namespace MVC.Authorization
{
    public class SignalRAuthorize : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            var token = request.QueryString.Get("bearer");

            var ticket = OAuthConfig.Options.AccessTokenFormat.Unprotect(token);

            if (ticket?.Identity == null || !ticket.Identity.IsAuthenticated)
            {
                return false;
            }

            request.Environment["user"] = new ClaimsPrincipal(ticket.Identity);

            return true;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;

            var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;

            var principal = environment["user"] as ClaimsPrincipal;

            if (principal?.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return false;
            }

            var request = new ServerRequest(environment);

            hubIncomingInvokerContext.Hub.Context = new HubCallerContext(request, connectionId);

            return true;
        }
    }
}