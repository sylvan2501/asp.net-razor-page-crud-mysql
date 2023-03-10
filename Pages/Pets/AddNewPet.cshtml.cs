using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
namespace WebAppMysql.Pages.Pets
{
  
	public class AddNewPetModel : PageModel
    {
        public PetInfo petInfo = new PetInfo();
        public string successMessage = "";
        public string errorMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            //model state validation in razor page
            //if (!ModelState.IsValid)
            //{
            //    errorMessage = "Unable to validate user inputs";
            //    return;
            //}
            petInfo.name = Request.Form["name"];
            petInfo.owner = Request.Form["owner"];
            petInfo.email = Request.Form["email"];
            petInfo.species = Request.Form["species"];
            petInfo.sex = Request.Form["sex"];
            petInfo.birth = Request.Form["birth"];
            //save the pet information to the database
            try {
                DotNetEnv.Env.Load();
                var password = Environment.GetEnvironmentVariable("PASSWORD");
                var server = Environment.GetEnvironmentVariable("SERVER");
                var user = Environment.GetEnvironmentVariable("USER");
                var database = Environment.GetEnvironmentVariable("DATABASE");
                String connectionString = $"server={server};user={user};password={password};database={database}";
                using MySqlConnection connection = new MySqlConnection(connectionString);
                {
                    connection.Open();
                    string query = "INSERT INTO Pets (name, owner, email, species, sex, birth) VALUES(" +
                        "@name, " +
                        "@owner, " +
                        "@email, " +
                        "@species, " +
                        "@sex, " +
                        "@birth);";

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
            } catch (Exception ex) {
                errorMessage = ex.Message;
                return;
            }



            //clear the fields of petInfo by creating a new object instance
            petInfo = new PetInfo();
            successMessage = "The record is successfully saved!";
            Response.Redirect("/Index");
            //ModelState.Clear();
        }

    }
}
