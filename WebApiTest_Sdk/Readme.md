
Links:
------
[http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/](http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/)


Route Attribute
---------------
[http://stackoverflow.com/questions/18778390/attributerouting-not-working-with-httpconfiguration-object-for-writing-integrati](http://stackoverflow.com/questions/18778390/attributerouting-not-working-with-httpconfiguration-object-for-writing-integrati)

[http://ifyoudo.net/post/2014/01/28/How-to-unit-test-ASPNET-Web-API-2-Route-Attributes](http://ifyoudo.net/post/2014/01/28/How-to-unit-test-ASPNET-Web-API-2-Route-Attributes)

[http://blogs.msdn.com/b/kiranchalla/archive/2012/05/06/in-memory-client-amp-host-and-integration-testing-of-your-web-api-service.aspx](http://blogs.msdn.com/b/kiranchalla/archive/2012/05/06/in-memory-client-amp-host-and-integration-testing-of-your-web-api-service.aspx)

Sdk:
----
[http://dontcodetired.com/blog/post/In-Process-Http-Server-for-Integration-Test-Faking-with-Owin-Katana-and-WebAPI.aspx](http://dontcodetired.com/blog/post/In-Process-Http-Server-for-Integration-Test-Faking-with-Owin-Katana-and-WebAPI.aspx)

Too use route attribute[Route("hello/get")] on web api actions-web.api
---------------------------------------------------------------
Install-Package Microsoft.AspNet.WebApi.WebHost


Ioc-web.api and test:
----
Install-Package Ninject

And mange the ninject scop class, iniject dependency resolver class


Test project:
-------------
Install-Package Microsoft.AspNet.WebApi.OwinSelfHost

Install-Package Microsoft.Owin.Testing


owin self host
--------------
Important run **Vs** on **administratior** mode, or some times throws errors