using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OdeToFood.Pages.Restaurants
{
    public class NotFoundModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}