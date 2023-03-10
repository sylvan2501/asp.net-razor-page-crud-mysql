using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using MySqlConnector;
using System.Data;
using WebAppMysql.Pages.CustomAuthorization;

namespace WebAppMysql.Pages.Account
{
	public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential credential { get; set; }


        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            //string uname = string.Empty;
            //string pw = string.Empty;
            //try
            //{
            //    DotNetEnv.Env.Load();
            //    var password = Environment.GetEnvironmentVariable("PASSWORD");
            //    var server = Environment.GetEnvironmentVariable("SERVER");
            //    var user = Environment.GetEnvironmentVariable("USER");
            //    var database = Environment.GetEnvironmentVariable("DATABASE");
            //    String connectionString = $"server={server};user={user};password={password};database={database}";
            //    using (MySqlConnection connection = new MySqlConnection(connectionString))
            //    {
            //        connection.Open();
            //        string query = "SELECT * FROM Users WHERE username = @username";
            //        using (MySqlCommand command = new MySqlCommand(query, connection))
            //        {
            //            command.Parameters.AddWithValue("@username", credential.UserName);
            //            using (MySqlDataReader reader = command.ExecuteReader())
            //            {
            //                if (reader.HasRows)
            //                {
            //                    while (reader.Read())
            //                    {
            //                        //Console.WriteLine(reader.GetString(1));
            //                        //Console.WriteLine(reader.GetString(2));
            //                        uname = reader.GetString(1);
            //                        pw = reader.GetString(2);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //verify the credential
            //Console.WriteLine(uname, pw);
            if (credential.UserName =="admin" /*uname*/ && credential.Password=="password"/*BCrypt.Net.BCrypt.Verify(credential.Password, pw)*/)
            {
                //Console.WriteLine("Successfully verified");
                //creating the security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@email.com"),
                    new Claim("Identity","Owner"),
                    new Claim("Identity","Authenticated"),
                    new Claim("Owner", "true"),
                    new Claim("FreeTrialStartDate", "2023-03-01")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = credential.RememberMe
                };


                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authenticationProperties);

                TempData["Success"] = "Success";
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }

    
}
