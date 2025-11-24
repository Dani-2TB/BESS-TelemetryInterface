using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using DotnetAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DotnetAPI.Pages.Auth;

public class IndexModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;

    public IndexModel(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; } = default!;

    public class InputModel
    {
        [Required]
        [DataType(DataType.Text), DisplayName("Username")]
        public string UserName { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }

    public void OnGet(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // If user is already logged in, redirect
        if (User.Identity?.IsAuthenticated ?? false)
        {
            Response.Redirect(returnUrl);
        }

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, isPersistent: true, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username or Password is not valid");
                return Page();
            }
        }

        return Page();
    }
}