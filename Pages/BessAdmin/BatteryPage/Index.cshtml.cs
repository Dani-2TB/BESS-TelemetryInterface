using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.BatteryPage
{
    public class IndexModel : PageModel
    {
        private readonly YuzzContext _context;

        public IndexModel(YuzzContext context)
        {
            _context = context;
        }

        public IList<Battery> Battery { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Battery = await _context.Batteries
                .Include(b => b.Bess)
                .ToListAsync();
        }
    }
}
