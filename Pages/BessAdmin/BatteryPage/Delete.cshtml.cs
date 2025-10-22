using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.Admin.BatteryPage
{
    public class DeleteModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DeleteModel(DotnetAPI.Data.YuzzContext context)
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

            var battery = await _context.Batteries.FirstOrDefaultAsync(m => m.Id == id);

            if (battery is not null)
            {
                Battery = battery;

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

            var battery = await _context.Batteries.FindAsync(id);
            if (battery != null)
            {
                Battery = battery;
                _context.Batteries.Remove(Battery);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
