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

});
builder.Services.AddAuthorization(options => {
    // examples of simple-policy-based autorization
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

app.MapRazorPages();

app.Run();

