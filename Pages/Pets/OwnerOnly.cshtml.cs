using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppMysql.Pages.Pets
{
    [Authorize(Policy="MustBeAnOwner")]
	public class OwnerOnlyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
