using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models.Domain;

namespace DotnetAPI.Pages.BessAdmin.BessPage
{
    public class EditModel : PageModel
    {
        private readonly YuzzContext _context;

        private readonly ILogger<EditModel> _logger;

        public EditModel(YuzzContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Bess Bess { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bess = await _context.Besses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bess == null)
            {
                return NotFound();
            }
            bess.CurrentMaxAcOut /= 1000;
            bess.CurrentMaxAcIn /= 1000;
            bess.ThresholdCurrent /= 1000;

            Bess = bess;
            ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Bess.CurrentMaxAcIn *= 1000;
            Bess.CurrentMaxAcOut *= 1000;
            Bess.ThresholdCurrent *= 1000;

            if (!ModelState.IsValid)
            {
                ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogError("ModelState error on '{Field}': {Error}",
                            key,
                            string.IsNullOrWhiteSpace(error.ErrorMessage)
                                ? error.Exception?.Message
                                : error.ErrorMessage);
                    }
                }
                return Page();
            }

            _context.Attach(Bess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BessExists(Bess.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/BessAdmin/Index");
        }

        private bool BessExists(int id)
        {
            return _context.Besses.Any(e => e.Id == id);
        }
    }
}
