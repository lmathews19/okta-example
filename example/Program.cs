using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Okta.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
})
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddOktaMvc(new OktaMvcOptions
           {
               // Replace these values with your Okta configuration
               OktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain"),
               AuthorizationServerId = builder.Configuration.GetValue<string>("Okta:AuthorizationServerId"),
               ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
               ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
               Scope = new List<string> { "openid", "profile", "email" },
           });

// Add services to the container.
builder.Services.AddRazorPages();

// Add the OktaAuthorizationFilter
builder.Services.AddScoped<OktaAuthorizationFilter>();

//Add the filter to the razor pages pipeline.
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<OktaAuthorizationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

// OktaAuthorizationFilter class
public class OktaAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        //Exclude login and error pages from authorization check.
        //if ((ShouldExcludePath(context.HttpContext.Request.Path))
        //{
        //    return;
        //}

        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            await context.HttpContext.ChallengeAsync(OktaDefaults.MvcAuthenticationScheme);
            context.Result = new ChallengeResult(OktaDefaults.MvcAuthenticationScheme);
        }
    }
















    private bool ShouldExcludePath(PathString path)
    {
        return path.StartsWithSegments("/Login") ||
               path.StartsWithSegments("/Error") ||
               path.StartsWithSegments("/css") ||
               path.StartsWithSegments("/js") ||
               path.StartsWithSegments("/lib");
    }
}