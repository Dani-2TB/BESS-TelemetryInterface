using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.PcsModelPage
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
            return Page();
        }

        [BindProperty]
        public PcsModel PcsModel { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PcsModels.Add(PcsModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
