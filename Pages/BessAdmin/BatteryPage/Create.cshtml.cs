using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.BatteryPage
{
    public class CreateModel : PageModel
    {
        private readonly YuzzContext _context;

        public CreateModel(YuzzContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BessId"] = new SelectList(_context.Besses, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Battery Battery { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["BessId"] = new SelectList(_context.Besses, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Batteries.Add(Battery);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
