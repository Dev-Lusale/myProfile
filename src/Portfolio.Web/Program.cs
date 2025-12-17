using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio.Web;
using Portfolio.Web.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with configuration-based URL
builder.Services.AddScoped(sp => 
{
    var httpClient = new HttpClient();
    
    // Get API base URL from configuration
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? 
        (builder.HostEnvironment.IsDevelopment() 
            ? "http://localhost:5000/" 
            : "https://bernard-portfolio-api.azurewebsites.net/");
    
    httpClient.BaseAddress = new Uri(apiBaseUrl);
    return httpClient;
});

// Add services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddSingleton<Portfolio.Shared.Services.SidebarService>();

await builder.Build().RunAsync();