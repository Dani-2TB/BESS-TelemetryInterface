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
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(YuzzContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
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
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogError("ModelState error on '{Field}': {Error}",
                            key,
                            string.IsNullOrWhiteSpace(error.ErrorMessage)
                                ? error.Exception?.Message
                                : error.ErrorMessage);
                    }
                }
                return Page();
            }

            // Scale before saving
            Battery.CurrentMax *= 1000;
            Battery.CurrentCharging *= 1000;

            Battery.VoltageMax *= 1000;
            Battery.VoltageMin *= 1000;
            Battery.VoltageAbsorption *= 1000;

            Battery.PwrMax *= 1000;

            Battery.SocMax *= 10;
            Battery.SocMin *= 10;

            if (Battery.SocMax < Battery.SocMin)
            {
                ModelState.AddModelError("", "Soc values ranges don't make sense");
                return Page();
            }

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
