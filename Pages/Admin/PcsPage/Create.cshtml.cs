using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.PcsPage
{
    public class CreateModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public CreateModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BatteryId"] = new SelectList(_context.Batteries, "Id", "Name");
        ViewData["PcsModelId"] = new SelectList(_context.PcsModels, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Pcs Pcs { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Pcs.Add(Pcs);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
