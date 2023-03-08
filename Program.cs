using Microsoft.AspNetCore.Authentication.JwtBearer;
using MySqlConnector;
var builder = WebApplication.CreateBuilder(args);

//register database connection
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(builder.Configuration.GetConnectionString("Default")));

//register authentication service
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.Cookie.Name = "MyCookieAuth";

});

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

