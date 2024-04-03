#pragma warning disable SA1200 // Using directives should be placed correctly
using IdentityServer4.Extensions;
using LearningHub.Nhs.Auth;
using LearningHub.Nhs.Auth.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
#pragma warning restore SA1200 // Using directives should be placed correctly

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets("a2ecb5d2-cf13-4551-9cb6-3d86dfbcf8ef");

builder.Logging.ClearProviders();

builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

builder.Logging.AddConsole();

builder.Host.UseNLog();

string corsOriginUrl = builder.Configuration.GetValue<string>("LearningHubAuthConfig:AuthClients:learninghubopenapi:BaseUrl");

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .WithOrigins(corsOriginUrl)
        .AllowAnyHeader());
});

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

GlobalDiagnosticsContext.Set("connectionString", builder.Configuration.GetSection("ASPNETCORE_ConnectionStrings")["NLogDb"]);

var app = builder.Build();

app.Use(
    async (ctx, next) =>
    {
        ctx.SetIdentityServerOrigin($"https://{app.Configuration.GetValue<string>("AuthOrigin")}");
        await next();
    });

app.Use(
    async (ctx, next) =>
    {
        ctx.Response.Headers["Access-Control-Allow-Origin"] = "*";
        ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PATCH, PUT, DELETE, OPTIONS";
        ctx.Response.Headers["Access-Control-Allow-Headers"] = "Origin, Content-Type, X-Auth-Token";
        await next();
    });

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseExceptionHandler("/Home/Error");
}

LogManager.Configuration.Variables["connectionString"] = app.Configuration.GetConnectionString("NLogDb");

app.UseCors("CorsPolicy");

app.UseCookiePolicy();

app.UseHttpsRedirection();

app.UseIdentityServer();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(
    endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapDefaultControllerRoute();
    });

app.Run();