using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.BessPage
{
    public class IndexModel : PageModel
    {
        private readonly YuzzContext _context;

        public IndexModel(YuzzContext context)
        {
            _context = context;
        }

        public IList<Bess> Bess { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Bess = await _context.Besses
                .Include(b => b.OperationMode)
                .ToListAsync();
        }
    }
}
