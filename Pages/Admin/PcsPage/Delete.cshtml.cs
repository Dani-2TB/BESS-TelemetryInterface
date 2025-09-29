using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.PcsPage
{
    public class DeleteModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DeleteModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pcs Pcs { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcs = await _context.Pcs.FirstOrDefaultAsync(m => m.Id == id);

            if (pcs is not null)
            {
                Pcs = pcs;

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

            var pcs = await _context.Pcs.FindAsync(id);
            if (pcs != null)
            {
                Pcs = pcs;
                _context.Pcs.Remove(Pcs);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
