using BusinessLogic.Repository;
using BusinessLogic.Services.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using MVC.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.App_Start
{
    public static class OAuthConfig
    {
        public static OAuthAuthorizationServerOptions Options { get; set; }

        public const string tokenPath = "/token";

        public const int tokenLifetimeInHours = 4;

        public const bool isInsecureHttpAllowed = true;

        static OAuthConfig()
        {
            Options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = isInsecureHttpAllowed,
                TokenEndpointPath = new PathString(tokenPath),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(tokenLifetimeInHours),
                Provider = new AuthorizationServerProvider(UnityConfig.GetConfiguredContainer().Resolve<IUserService>())
            };
        }
    }
}