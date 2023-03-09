using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace WebAppMysql.Pages.Pets
{

    [Authorize(Policy = "MustBeAuthenticated")]
    public class PetListModel : PageModel
    {
        public List<PetInfo> petList = new List<PetInfo>();
        public void OnGet()
        {
            try {
                DotNetEnv.Env.Load();
                var password = Environment.GetEnvironmentVariable("PASSWORD");
                var server = Environment.GetEnvironmentVariable("SERVER");
                var user = Environment.GetEnvironmentVariable("USER");
                var database = Environment.GetEnvironmentVariable("DATABASE");
                string connectionString = $"server={server};user={user};password={password};database={database}";
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM Pets";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PetInfo petInfo = new PetInfo();
                            petInfo.id = reader.GetInt16(0);
                            petInfo.name = reader.GetString(1);
                            petInfo.owner = reader.GetString(2);
                            petInfo.email = reader.GetString(3);
                            petInfo.species = reader.GetString(4);
                            petInfo.sex = reader.GetString(5);
                            petInfo.birth = reader.GetDateTime(6).ToString();
                            petList.Add(petInfo);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Exception: " +  ex.ToString());
            }
        }
    }
    public class PetInfo
    {
        public int id;
        public string name { get; set; }
        public string owner { get; set; }
        public string email { get; set; }
        public string species { get; set; }
        public string sex { get; set; }
        public string birth { get; set; }
    }
}
