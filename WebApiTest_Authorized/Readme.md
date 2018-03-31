
Links:
------
[http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/](http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/)


Route Attribute
---------------
[http://stackoverflow.com/questions/18778390/attributerouting-not-working-with-httpconfiguration-object-for-writing-integrati](http://stackoverflow.com/questions/18778390/attributerouting-not-working-with-httpconfiguration-object-for-writing-integrati)

[http://ifyoudo.net/post/2014/01/28/How-to-unit-test-ASPNET-Web-API-2-Route-Attributes](http://ifyoudo.net/post/2014/01/28/How-to-unit-test-ASPNET-Web-API-2-Route-Attributes)

[http://blogs.msdn.com/b/kiranchalla/archive/2012/05/06/in-memory-client-amp-host-and-integration-testing-of-your-web-api-service.aspx](http://blogs.msdn.com/b/kiranchalla/archive/2012/05/06/in-memory-client-amp-host-and-integration-testing-of-your-web-api-service.aspx)


Token Auth:
-----------
[http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api](http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api)

[http://bitoftech.net/2015/02/16/implement-oauth-json-web-tokens-authentication-in-asp-net-web-api-and-identity-2/](http://bitoftech.net/2015/02/16/implement-oauth-json-web-tokens-authentication-in-asp-net-web-api-and-identity-2/)

[http://blog.kloud.com.au/2014/10/26/asp-net-web-api-integration-testing-with-one-line-of-code/](http://blog.kloud.com.au/2014/10/26/asp-net-web-api-integration-testing-with-one-line-of-code/)


Claim:
------
http://www.jayway.com/2014/09/25/securing-asp-net-web-api-endpoints-using-owin-oauth-2-0-and-claims/


How request token to auth provider:
-------------------------------------
[http://stackoverflow.com/questions/29246908/c-sharp-unsupported-grant-type-when-calling-web-api](http://stackoverflow.com/questions/29246908/c-sharp-unsupported-grant-type-when-calling-web-api)


Working with exceptions on HttpClient:
--------------------------------------
[http://stackoverflow.com/questions/12103946/httpclient-doesnt-report-exception-returned-from-web-api](http://stackoverflow.com/questions/12103946/httpclient-doesnt-report-exception-returned-from-web-api)


To use route attribute[Route("hello/get")] on web api actions-web.api
---------------------------------------------------------------
Install-Package Microsoft.AspNet.WebApi.WebHost


To use owin auth on web.api
-----------------------------
Install-Package Microsoft.AspNet.WebApi.Owin 

Install-Package Microsoft.Owin.Host.SystemWeb 

Install-Package Microsoft.Owin.Cors 

Install-Package Microsoft.Owin.Security.OAuth 

Install-Package System.IdentityModel.Tokens.Jwt 

Install-Package Thinktecture.IdentityModel.Core 

Install-Package Microsoft.Owin.Security.Jwt


to use Identity class on web api project
----------------------------------------
Install-Package Microsoft.AspNet.Identity.EntityFramework  

this will install entityframewrok too


to use app.CreatePerOwinContext() on web api poject:
---------------------------------------------------- 
Install-Package Microsoft.AspNet.Identity.Owin


Ioc-web.api and test:
---------------------
Install-Package Ninject

And mange the ninject scop class, iniject dependency resolver class


Test project:
-------------
Install-Package Microsoft.AspNet.WebApi.OwinSelfHost

Install-Package Microsoft.Owin.Testing

also mock, nbuilder

Token Auth in Core:
-------------------
[https://github.com/foyzulkarim/Cores]
(https://github.com/foyzulkarim/Cores)
