using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.BessPage
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
        ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Bess Bess { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Besses.Add(Bess);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
