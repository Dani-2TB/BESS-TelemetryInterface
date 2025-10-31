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

namespace DotnetAPI.Pages.BessAdmin.BatteryPage
{
    public class EditModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public EditModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Battery Battery { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battery =  await _context.Batteries.FirstOrDefaultAsync(m => m.Id == id);
            if (battery == null)
            {
                return NotFound();
            }

            // Scale before displaying.
            battery.CurrentMax /= 1000;
            battery.CurrentCharging /= 1000;

            battery.VoltageMax /= 1000;
            battery.VoltageMin /= 1000;
            battery.VoltageAbsorption /= 1000;

            battery.PwrMax /= 1000;


            Battery = battery;
            ViewData["BessId"] = new SelectList(_context.Besses, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["BessId"] = new SelectList(_context.Besses, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Scale before saving
            Battery.CurrentMax *= 1000;
            Battery.CurrentCharging *= 1000;

            Battery.VoltageMax *= 1000;
            Battery.VoltageMin *= 1000;
            Battery.VoltageAbsorption *= 1000;

            Battery.PwrMax *= 1000;
            
            _context.Attach(Battery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatteryExists(Battery.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error updating PCS. Make sure the ID is unique.");
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private bool BatteryExists(int id)
        {
            return _context.Batteries.Any(e => e.Id == id);
        }
    }
}
