using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoRazorPageAuthorise.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult OnGet()
        {

            return Page();
        }
    }
}
