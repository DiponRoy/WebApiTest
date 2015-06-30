using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using Db.Model;
using FizzWare.NBuilder;
using NUnit.Framework;
using Web.Api.Auth;
using Web.Api.Controllers;
using Web.Api.Model.Response;

namespace Web.Api.Tests.UnitTest.WithOutServer
{
    [TestFixture]
    public class HelloControllerTest
    {

        [Test]
        /*Http response unit test*/
        public void AnonymousHttp_Success()
        {
            //setting claim at controller
            var controller = new HelloController();
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            //test
            var response = controller.AnonymousGetHttpSuccess();
            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(response);
            var value = (OkNegotiatedContentResult<string>) response;
            Assert.AreEqual("Success.", value.Content);

        }

        [Test]
        /*Http error unit test*/
        public void AnonymousHttp_Error()
        {
            //setting http at controller
            var controller = new HelloController();
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            //test
            var error = Assert.Catch<HttpResponseException>(() => controller.AnonymousGetHttpError());
            Assert.AreEqual(400, (int) error.Response.StatusCode);
            Assert.AreEqual("Error.", error.Response.Content.ReadAsAsync<string>().Result);
        }

        [Test]
        /*using claim during unit test*/
        public void Identity()
        {
            Admin admin = Builder<Admin>.CreateNew().Build();
            ClaimsIdentity identity = AuthUtility.Identity(admin);

            //setting claim at controller
            var controller = new HelloController();
            controller.User = new ClaimsPrincipal(identity);

            //test
            ApiResponse<Admin> response = controller.GetIdentity();
            Admin result = response.Data;
            Assert.AreEqual(admin.Id, result.Id);
            Assert.AreEqual(admin.LoginName, result.LoginName);
        }
    }
}
