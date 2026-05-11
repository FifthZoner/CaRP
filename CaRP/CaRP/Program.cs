using CaRP.Backend;
using CaRP.Components;
using Clerk.Net.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

public class Program
{

public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRazorComponents()
        .AddInteractiveWebAssemblyComponents();

    // Configuration
    var secretKey = Endpoints.Secrets.SecretKey = builder.Configuration["Clerk:SecretKey"] ?? "";
    var publishableKey = Endpoints.Secrets.PublishableKey = builder.Configuration["Clerk:PublishableKey"] ?? "";
    var connString = Endpoints.Secrets.ConnString = builder.Configuration["Database:ConnString"] ?? "";

    // Database
    builder.Services.AddDbContext<DbContext>(options =>
        options.UseNpgsql(connString));

    // Clerk API Client (Using the official DI helper)
    builder.Services.AddClerkApiClient(config =>
    {
        config.SecretKey = secretKey;
    });

    // Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.Authority = "https://becoming-sole-26.clerk.accounts.dev";
            options.MetadataAddress = "https://becoming-sole-26.clerk.accounts.dev/.well-known/openid-configuration";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://becoming-sole-26.clerk.accounts.dev",
                ValidateAudience = false,
                ValidateLifetime = true
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddCors(options => {
        options.AddPolicy("AllowBlazor", policy => {
            policy.WithOrigins("https://localhost:5249") // Your Blazor Port
                .AllowAnyMethod()
                .AllowAnyHeader() // CRITICAL: This allows the 'Authorization' header
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.MapStaticAssets();

    app.UseRouting();
    app.UseCors("AllowBlazor");

    app.UseAuthentication(); // MUST be before Authorization
    app.UseAuthorization();
    app.UseAntiforgery();

    Endpoints.MapEndpoints(app.MapGroup("/api"));

    app.MapRazorComponents<App>()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(CaRP.Client._Imports).Assembly);

    app.Run();
}
}
