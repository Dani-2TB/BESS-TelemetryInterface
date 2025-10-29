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

namespace DotnetAPI.Pages.BessAdmin.PcsPage
{
    public class EditModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public EditModel(DotnetAPI.Data.YuzzContext context)
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

            var pcs =  await _context.Pcs.FirstOrDefaultAsync(m => m.Id == id);
            if (pcs == null)
            {
                return NotFound();
            }
            Pcs = pcs;
            ViewData["BatteryId"] = new SelectList(_context.Batteries, "Id", "Name");
            ViewData["PcsModelId"] = new SelectList(_context.PcsModels, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["BatteryId"] = new SelectList(_context.Batteries, "Id", "Name");
                ViewData["PcsModelId"] = new SelectList(_context.PcsModels, "Id", "Name");
                return Page();
            }

            _context.Attach(Pcs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcsExists(Pcs.Id))
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

        private bool PcsExists(int id)
        {
            return _context.Pcs.Any(e => e.Id == id);
        }
    }
}
