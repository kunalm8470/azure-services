using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IConfiguration Configuration;
        private static IConfigurationRefresher _configurationRefresher;

        protected void Application_Start()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)
                        .ConfigureRefresh(refresh =>
                        {
                            refresh.Register("appconf:sentinelKey", refreshAll: true)
                                   .SetCacheExpiration(new TimeSpan(0, 5, 0));
                        });
                _configurationRefresher = options.GetRefresher();
            });

            Configuration = builder.Build();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _ = _configurationRefresher.TryRefreshAsync();
        }
    }
}
