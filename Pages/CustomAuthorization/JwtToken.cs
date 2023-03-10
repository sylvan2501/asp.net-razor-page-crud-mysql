using System;
using Newtonsoft.Json;

namespace WebAppMysql.Pages.CustomAuthorization
{
	public class JwtToken
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		[JsonProperty("expires_at")]
		public DateTime ExpireAt { get; set; }
		public JwtToken()
		{
		}
	}
}

