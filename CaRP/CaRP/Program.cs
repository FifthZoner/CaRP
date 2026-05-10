using CaRP.Backend;
using CaRP.Components;
using Clerk.Net.Client;
using Clerk.Net.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using System.Net.Http.Headers;

namespace CaRP;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        Endpoints.Secrets.SecretKey = builder.Configuration["Clerk:SecretKey"] ?? "";
        Endpoints.Secrets.PublishableKey = builder.Configuration["Clerk:PublishableKey"] ?? "";
        Endpoints.Secrets.ConnString = builder.Configuration["Database:ConnString"] ?? "";


        builder.Services.AddDbContext<DbContext>(options =>
            options.UseNpgsql(Endpoints.Secrets.ConnString));

        builder.Services.AddClerkApiClient(config =>
        {
            config.SecretKey = Endpoints.Secrets.SecretKey;
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.Authority = "https://becoming-sole-26.clerk.accounts.dev";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization();

        var authProvider = new ApiKeyAuthenticationProvider(
            Endpoints.Secrets.SecretKey,
            "Authorization",
            ApiKeyAuthenticationProvider.KeyLocation.Header,
            "Bearer");

        var adapter = new HttpClientRequestAdapter(authProvider)
        {
            BaseUrl = "https://api.clerk.com/v1"
        };

        builder.Services.AddSingleton(new ClerkApiClient(adapter));

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://localhost:5249")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        app.UseRouting();
        app.UseCors(); // Must be placed between UseRouting and UseAuthorization
        app.UseAuthorization();

        Endpoints.MapEndpoints(app.MapGroup("/api"));

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.UseWebAssemblyDebugging();
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.Run();
    }
}