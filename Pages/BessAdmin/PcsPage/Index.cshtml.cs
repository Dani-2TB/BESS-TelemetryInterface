using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.PcsPage
{
    public class IndexModel : PageModel
    {
        private readonly DotnetAPI.Data.YuzzContext _context;

        public IndexModel(DotnetAPI.Data.YuzzContext context)
        {
            _context = context;
        }

        public IList<Pcs> Pcs { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Pcs = await _context.Pcs
                .Include(p => p.Battery)
                .Include(p => p.PcsModel).ToListAsync();
        }
    }
}
