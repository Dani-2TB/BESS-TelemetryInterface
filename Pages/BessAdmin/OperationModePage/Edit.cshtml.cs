using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.OperationModePage
{
    public class EditModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public EditModel(DotnetAPI.Data.YuzzContext context)
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

            var operationmode =  await _context.OperationModes.FirstOrDefaultAsync(m => m.Id == id);
            if (operationmode == null)
            {
                return NotFound();
            }
            OperationMode = operationmode;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OperationMode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationModeExists(OperationMode.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OperationModeExists(int id)
        {
            return _context.OperationModes.Any(e => e.Id == id);
        }
    }
}
