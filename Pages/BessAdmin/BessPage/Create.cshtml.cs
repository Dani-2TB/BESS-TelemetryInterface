using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.BessPage
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
        ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Bess Bess { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Besses.Add(Bess);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
