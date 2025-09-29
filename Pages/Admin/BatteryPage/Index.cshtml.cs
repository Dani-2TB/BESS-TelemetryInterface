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
    public class IndexModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public IndexModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        public IList<Battery> Battery { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Battery = await _context.Batteries
                .Include(b => b.Bess).ToListAsync();
        }
    }
}
