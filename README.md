# BlazorAuth0Demo
Auhentication and Authorization with Blazor WebAssembly and Auth0 Identity Provider

The demo application is a demo/test-application to work with Auth0 (https://auth0.com) as identity provider for authentication and authorization for a typical Blazor application. 
The application is created step by step:
* Project template "Blazor WebAssembly App ASP.NET Core hosted" as starting point.
* Adoption of article https://auth0.com/blog/securing-blazor-webassembly-apps/

After these steps the application supports authentication including sign-up and authorization at the backend if calling the "WeatherForecast". See the page <a href="https://github.com/rbrands/BlazorAuth0Demo/blob/master/BlazorAuth0Demo/Client/Pages/Claims.razor">"Claims"</a> how to use
the Authorize attribute and use the CascadingParameter AuthenticationState to get the user as ClaimsPrincipal, see https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-3.1

Auth0 supports permissions on APIs that can be assigned to users. These permissions are handled as "scopes". To use these scopes for authorization with policies (https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-3.1) on server side and client side follow these steps:
* See article https://stackoverflow.com/questions/61153224/use-authorization-roles-and-policies-in-blazor-webassembly-with-identity as starting point.
* In <a href="https://github.com/rbrands/BlazorAuth0Demo/tree/master/BlazorAuth0Demo/Shared">BlazorAuth0Demo.Shared</a> add reference to package Microsoft.AspNetCore.Authorization to Shared and create policy related classes HasScopeRequirement, HasScopeHandler and PolicyTypes: Through the BlazorAuth0Demo.Shared project theses classes are available in client and server. See also https://auth0.com/docs/quickstart/backend/aspnet-core-webapi/01-authorization
* In static class PolicyTypes define names for policies and register them.
* In <a href="https://github.com/rbrands/BlazorAuth0Demo/blob/master/BlazorAuth0Demo/Server/Startup.cs">Server Startup.cs</a> register application policies (via class PolicyTypes) and IAuthorizationHandler as HasScopeHandler.
* In <a href="https://github.com/rbrands/BlazorAuth0Demo/blob/master/BlazorAuth0Demo/Client/Program.cs">Client Program.cs</a> PolicyTypes.READ_WEATHER etc to DefaultScopes requested. Register app policies and register IAuthorizationHandler. Add builder.Services.AddApiAuthorization().
* To get access tokens for outgoing calls to the api controller see https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-3.1#attach-tokens-to-outgoing-requests. Add package Microsoft.Extensions.Http and HttpFactory in Program.cs

At this moment evaluation of policies at client side does not work probable because Auth0 that not provides scope claims in the ID token used for authentication. That means that ony status "authenticated" can be used with Blazor standard templates. To check if a user has granted a permission follow the approach as shown in <a href="https://github.com/rbrands/BlazorAuth0Demo/blob/master/BlazorAuth0Demo/Client/Pages/FetchData.razor">FetchData.razor</a>:
* Request access token via TokenProvider and check if the required scope is granted.

General: Configure at your Auth0 tenant a default api like https://generic-api because currently there is no possibility to use "audience" during authentication.

