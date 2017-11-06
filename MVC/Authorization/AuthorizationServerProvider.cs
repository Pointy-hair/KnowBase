using BusinessLogic.DBContext;
using BusinessLogic.Repository;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Services.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserService userService;

        public AuthorizationServerProvider(IUserService userService)
        {
            this.userService = userService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = await userService.GetByLogin(context.UserName);

            if (user == null)
            {
                context.Rejected();

                context.SetError("User doesn't exist.", "Username or password is incorrect.");
            }
            else
            {
                if (MD5Hash.Verify(context.Password, user.Password))
                {
                    SetParams(context, user);

                    var identity = ConfigureClaims(context.Options.AuthenticationType, user);

                    context.Validated(identity);
                }
                else
                {
                    context.Rejected();

                    context.SetError("User doesn't exist.", "Username or password is incorrect.");
                }
            }

            return;
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            var id = context.OwinContext.Get<string>("id");

            var name = context.OwinContext.Get<string>("name");

            var role = context.OwinContext.Get<string>("role");

            var picture = context.OwinContext.Get<string>("picture");

            context.AdditionalResponseParameters.Add("id", id);

            context.AdditionalResponseParameters.Add("name", name);

            context.AdditionalResponseParameters.Add("role", role);

            context.AdditionalResponseParameters.Add("picture", picture);

            return base.TokenEndpointResponse(context);
        }


        private void SetParams(OAuthGrantResourceOwnerCredentialsContext context, User user)
        {
            context.OwinContext.Set<string>("id", user.Id.ToString());

            context.OwinContext.Set<string>("name", user.Name);

            context.OwinContext.Set<string>("role", user.UserRole.Id.ToString());

            context.OwinContext.Set<string>("picture", user.Picture);
        }

        private ClaimsIdentity ConfigureClaims(string authenticationType, User user)
        {
            var identity = new ClaimsIdentity(authenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.Id.ToString()));

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return identity;
        }
    }
}