using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProductCatalog.Views.products
{
    public class CreateModel : PageModel
    {
        // GET: Create a new product
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            } else
            {
                // Add the new product to the catalog


                // Redirect to the product list page
                return RedirectToPage("/Index");
            }
        }

    }
}
