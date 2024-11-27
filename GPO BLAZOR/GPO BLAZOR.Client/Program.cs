using GPO_BLAZOR.Client.Class.JSRunTimeAccess;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace GPO_BLAZOR.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddScoped<AuthenticationStateProvider, IdentetyAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<CookieStorageAccessor>();
            builder.Services.AddScoped<LocalStorageAccessor>();

            await builder.Build().RunAsync();
        }
    }

    public class IdentetyAuthenticationStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(new System.Security.Claims.ClaimsPrincipal());
        }
    }
}
