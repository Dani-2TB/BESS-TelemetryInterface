using Microsoft.EntityFrameworkCore;
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
                Console.WriteLine("Error");
                return Page();
            }

            // Scale before saving
            Battery.CurrentMax *= 1000;
            Battery.CurrentCharging *= 1000;

            Battery.VoltageMax *= 1000;
            Battery.VoltageMin *= 1000;
            Battery.VoltageAbsorption *= 1000;

            Battery.PwrMax *= 1000;
            
            try
            {
                _context.Batteries.Add(Battery);
                await _context.SaveChangesAsync();

            } catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error updating PCS. Make sure the ID is unique.");
                return Page();
            }



            return RedirectToPage("./Index");
        }
    }
}
