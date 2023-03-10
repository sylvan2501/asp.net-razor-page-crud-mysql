using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAppMysql.DTO;
using WebAppMysql.Pages.Account;
using WebAppMysql.Pages.CustomAuthorization;

namespace WebAppMysql.Pages.Pets
{
    [Authorize(Policy = "PlaygroundOnly")]
	public class PlaygroundModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        [BindProperty]
        public List<WeatherForecastDTO> WeatherForecastItems { get; set; }
        public PlaygroundModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task OnGetAsync()
        {
            //obtain the service items
           
            WeatherForecastItems = await InvokeEndPoint<List<WeatherForecastDTO>>("MyWebAPI","WeatherForecast");
        }

        private async Task<T> InvokeEndPoint<T>(string clientName, string url)
        {
            //obtain token from session
            JwtToken token = null;

            var strTokenObj = HttpContext.Session.GetString("access_token");
            if (string.IsNullOrWhiteSpace(strTokenObj))
            {
                token = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj);
            }
            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpireAt <= DateTime.UtcNow)
            {
                token = await Authenticate();
            }
            var httpClient = httpClientFactory.CreateClient(clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return await httpClient.GetFromJsonAsync<T>(url);
        }

        private async Task<JwtToken> Authenticate()
        {
            var httpClient = httpClientFactory.CreateClient("MyWebAPI");
            var res = await httpClient.PostAsJsonAsync("auth", new Credential { UserName = "admin", Password = "password" });
            res.EnsureSuccessStatusCode();
            string strJwt = await res.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", strJwt);
            return JsonConvert.DeserializeObject<JwtToken>(strJwt);
        }
    }
}
