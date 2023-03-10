using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

namespace WebAppMysql.Pages.Pets
{
	public class EditPetModel : PageModel
    {
        public string successMessage = "";
        public string errorMessage = "";
        public PetInfo petInfo = new PetInfo();
        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                DotNetEnv.Env.Load();
                var password = Environment.GetEnvironmentVariable("PASSWORD");
                var server = Environment.GetEnvironmentVariable("SERVER");
                var user = Environment.GetEnvironmentVariable("USER");
                var database = Environment.GetEnvironmentVariable("DATABASE");
                String connectionString = $"server={server};user={user};password={password};database={database};Allow User Variables=True";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Pets WHERE pet_id=@id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                petInfo.id = reader.GetInt16(0);
                                petInfo.name = reader.GetString(1);
                                petInfo.owner = reader.GetString(2);
                                petInfo.email = reader.GetString(3);
                                petInfo.species = reader.GetString(4);
                                petInfo.sex = reader.GetString(5);
                                petInfo.birth = reader.GetDateTime(6).ToString();
                            }

                        }
                    }
                }
            } catch (Exception ex)
            {
                
            }
        }
        public void OnPost()
        {
            petInfo.id = Convert.ToInt16(Request.Form["id"]);
            petInfo.name = Request.Form["name"];
            petInfo.owner = Request.Form["owner"];
            petInfo.email = Request.Form["email"];
            petInfo.species = Request.Form["species"];
            petInfo.sex = Request.Form["sex"];
            petInfo.birth = Request.Form["birth"];
            string id = Request.Form["id"];
            try
            {
                DotNetEnv.Env.Load();
                var password = Environment.GetEnvironmentVariable("PASSWORD");
                var server = Environment.GetEnvironmentVariable("SERVER");
                var user = Environment.GetEnvironmentVariable("USER");
                var database = Environment.GetEnvironmentVariable("DATABASE");
                String connectionString = $"server={server};user={user};password={password};database={database};Allow User Variables=True";
                using MySqlConnection connection = new MySqlConnection(connectionString);
                {
                    connection.Open();
                    Console.WriteLine("Database connected !!!");
                    string query = "UPDATE Pets SET name=@name, " +
                        "owner=@owner, " +
                        "email=@email, " +
                        "species=@species, " +
                        $"sex=@sex, birth=@birth WHERE pet_id={id};";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", petInfo.name);
                        command.Parameters.AddWithValue("@owner", petInfo.owner);
                        command.Parameters.AddWithValue("@email", petInfo.email);
                        command.Parameters.AddWithValue("@species", petInfo.species);
                        command.Parameters.AddWithValue("@sex", petInfo.sex);
                        command.Parameters.AddWithValue("@birth", petInfo.birth);

                        command.ExecuteNonQuery();
                    }
                }
            } catch (Exception ex)
            {
                errorMessage = ex.Message;
                //Console.WriteLine(errorMessage);
                return;
            }
            Response.Redirect("/Index");
        }
    }
}
