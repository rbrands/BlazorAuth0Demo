# BlazorAuth0Demo
Auhentication and Authorization with Blazor WebAssembly and Auth0 Identity Provider

The demo application is a demo/test-application to work with Auth0 (https://auth0.com) as identity provider for authentication and authorization for a typical Blazor application. 
The application is created step by step:
* Project template "Blazor WebAssembly App ASP.NET Core hosted" as starting point.
* Adoption of article https://auth0.com/blog/securing-blazor-webassembly-apps/

After these steps the application supports authentication including sign-up and authorization at the backend if calling the "WeatherForecast". See the page "Claims" how to use
the Authorize attribute and use the CascadingParameter AuthenticationState to get the user as ClaimsPrincipal, see https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-3.1

Auth0 supports permissions on APIs that can be assigned to users. These permissions are handled as "scopes". To check these scopes as policies in the authorization framework of 
ASP.NET Core on server side and client side follow these steps:
