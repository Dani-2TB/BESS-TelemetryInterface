using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.PcsModelPage
{
    public class DeleteModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DeleteModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PcsModel PcsModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsmodel = await _context.PcsModels.FirstOrDefaultAsync(m => m.Id == id);

            if (pcsmodel is not null)
            {
                PcsModel = pcsmodel;

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

            var pcsmodel = await _context.PcsModels.FindAsync(id);
            if (pcsmodel != null)
            {
                PcsModel = pcsmodel;
                _context.PcsModels.Remove(PcsModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
