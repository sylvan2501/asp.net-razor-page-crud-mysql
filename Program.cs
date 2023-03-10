using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using WebAppMysql.Pages.CustomAuthorization;
using static WebAppMysql.Pages.CustomAuthorization.FreeTrialExperienceRequirement;

var builder = WebApplication.CreateBuilder(args);

//register database connection
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration.GetConnectionString("Default")));

//register authentication service
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    //set the cookie lifetime
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});
builder.Services.AddAuthorization(options => {
    // examples of simple-policy-based autorization
    options.AddPolicy("AdminOnly",policy => policy.RequireClaim("Admin"));
    options.AddPolicy("MustBeAnOwner", policy => policy.RequireClaim("Identity", "Owner"));
    options.AddPolicy("MustBeAuthenticated", policy => policy.RequireClaim("Identity", "Authenticated"));
    //a custom-policy-based authorization 
    options.AddPolicy("PlaygroundOnly", policy => policy.
    RequireClaim("Identity", "Owner").RequireClaim("Owner").Requirements.Add(new FreeTrialExperienceRequirement(3)));

});

builder.Services.AddSingleton<IAuthorizationHandler, FreeTrialExperienceRequirementHandler>();
//builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, OptionsBuilderConfigurationExtensions =>
//{
//});


// Add services to the container.
builder.Services.AddRazorPages();
//Add/register http client services
builder.Services.AddHttpClient("MyWebAPI", client =>
{
    //the info, especially the last 5 digits(ssl port number)
    //can be found in the api app in launchSettings.json under under Properties folder
    client.BaseAddress = new Uri("https://localhost:7258/"); 
});
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

//inject authentication service to the middleware pipeline
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.Run();

