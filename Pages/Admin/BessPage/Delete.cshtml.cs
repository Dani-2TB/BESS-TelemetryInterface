using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.BessPage
{
    public class DeleteModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DeleteModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bess Bess { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bess = await _context.Besses.FirstOrDefaultAsync(m => m.Id == id);

            if (bess is not null)
            {
                Bess = bess;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bess = await _context.Besses.FindAsync(id);
            if (bess != null)
            {
                Bess = bess;
                _context.Besses.Remove(Bess);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
