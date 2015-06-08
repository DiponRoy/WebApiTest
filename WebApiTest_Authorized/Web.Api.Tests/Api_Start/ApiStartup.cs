using System.Web.Http;
using System.Web.Mvc;
using Owin;

namespace Web.Api.Tests.Api_Start
{
    public class ApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            IocConfig.Register(config);
            AuthConfig.Configure(app);

            app.UseWebApi(config);
        }
    }
}