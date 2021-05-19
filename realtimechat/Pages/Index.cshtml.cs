using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace realtimechat.Pages
{
    public class IndexModel : PageModel
    {
        public static int numOfInstant = 1;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            HttpContext.Session.SetString("user", "set user from index " + numOfInstant++);
        }
    }
}
