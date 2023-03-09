using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppMysql.Pages.Pets
{
    [Authorize(Policy = "PlaygroundOnly")]
	public class PlaygroundModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
