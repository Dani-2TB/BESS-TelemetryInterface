using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.OperationModePage
{
    public class DeleteModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DeleteModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OperationMode OperationMode { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationmode = await _context.OperationModes.FirstOrDefaultAsync(m => m.Id == id);

            if (operationmode is not null)
            {
                OperationMode = operationmode;

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

            var operationmode = await _context.OperationModes.FindAsync(id);
            if (operationmode != null)
            {
                OperationMode = operationmode;
                _context.OperationModes.Remove(OperationMode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
