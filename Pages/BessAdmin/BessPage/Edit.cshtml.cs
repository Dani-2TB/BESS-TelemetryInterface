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

        public EditModel(YuzzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bess Bess { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bess =  await _context.Besses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bess == null)
            {
                return NotFound();
            }
            Bess = bess;
            ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {entry.Key}, Error: {error.ErrorMessage}");
                    }
                }
                ViewData["OperationModeId"] = new SelectList(_context.OperationModes, "Id", "Name");
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
