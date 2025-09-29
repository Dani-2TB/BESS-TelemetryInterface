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
    public class DetailsModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public DetailsModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

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
    }
}
