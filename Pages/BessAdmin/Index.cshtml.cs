using DotnetAPI.Data;
using DotnetAPI.Models.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Pages.BessAdmin;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly YuzzContext _context;

    public IndexModel(ILogger<IndexModel> logger, YuzzContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Bess Bess { get; set; } = default!;
    public int NumberOfBatteries = 0;
    public int NumberOfPcs = 0;

    public async Task OnGetAsync()
    {
        Bess = await _context.Besses
        .Include(o => o.OperationMode)
        .SingleAsync(b => b.Id == 1);

        NumberOfBatteries = await _context.Batteries
            .Where(b => b.BessId == Bess.Id)
            .CountAsync();

        NumberOfPcs = await _context.Pcs
            .Include(p => p.Battery)
            .Where(p => p.Battery.BessId == Bess.Id)
            .CountAsync();
    }
}