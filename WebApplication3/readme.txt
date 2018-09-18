--mysqlconnection£º
Install-Package MySql.Data.EntityFrameworkCore -Pre
--
Install-Package log4net
--
Install-Package Autofac
Install-Package Autofac.Configuration
Install-Package Autofac.Extensions.DependencyInjection
-- autofac aop
Install-Package Autofac.Extras.DynamicProxy

-- protobuf Serializer
Install-Package protobuf-net

-- https (just mark here, haven't test)
Install-Package Microsoft.AspNetCore.Server.Kestrel.Https

-- redis (ServiceStack)
Install-Package ServiceStack.Redis

-- redis (StackExchange) (StackExchange.Redis.Mono can run under linux system)
--StackExchange.Redis can be installed via the nuget UI (as StackExchange.Redis), or via the nuget package manager console:
Install-Package StackExchange.Redis
--If you require a strong-named package (because your project is strong-named), then you may wish to use instead:
Install-Package StackExchange.Redis.StrongName


--identityserver4
IdentityServer4
IdentityServer4.AspNetIdentity ()

IdentityModel
IdentityServer4.AccessTokenValidation
---- for openid
Microsoft.AspNetCore.Authentication.Cookies
Microsoft.AspNetCore.Authentication.OpenIdConnect

-- xunit
Install-Package xunit
Install-Package xunit.abstractions
Install-Package xunit.runner.visualstudio
Install-Package moq

-- chinese <-> pinyin, simple <-> complex
Install-Package PinYinConverterCore
Install-Package TraditionalChineseToSimplifiedConverter