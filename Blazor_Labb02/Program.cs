using Blazor_Labb02;
using Blazor_Labb02.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(
    sp => new HttpClient {
        //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
        BaseAddress = new Uri("https://localhost:7108/")
    }
);

builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CartService>();


await builder.Build().RunAsync();
