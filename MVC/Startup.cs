using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using MVC.Authorization;
using BusinessLogic.Repository;
using Microsoft.Practices.Unity;
using MVC.App_Start;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessLogic;

[assembly: OwinStartup(typeof(MVC.Startup))]
namespace MVC
{
    public class Startup
    {
        public const string tokenPath = "/token";

        public const int tokenLifetimeInHours = 4;

        public const bool isInsecureHttpAllowed = true;

        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ConfigureUnityContainer();

            app.UseWebApi(config);

            AutoMapperConfig.Configure();

            app.MapSignalR();
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions OAuthServerOptions = OAuthConfig.Options;

            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureUnityContainer()
        {
            using (var container = new UnityContainer())
            {
                UnityConfig.RegisterTypes(container);

                UnityWebActivator.Start();
            }
        }
    }
}