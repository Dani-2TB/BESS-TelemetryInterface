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
    public class DetailsModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DetailsModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

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
    }
}
