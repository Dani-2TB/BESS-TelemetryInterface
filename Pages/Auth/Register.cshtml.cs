using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotnetAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace DotnetAPI.Pages.Auth;

[Authorize(Roles = "Admin")]
public class RegisterModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RegisterModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    public SelectList RoleOptions { get; set; } = default!;

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;

        // Custom Fields
        [Required]
        public int Rut { get; set; }

        [Required]
        [StringLength(1)]
        public string Dv { get; set; } = default!;

        [Required]
        [Display(Name = "Full Name")]
        public string NombreCompleto { get; set; } = default!;

        [Display(Name = "Job Title")]
        public string? Cargo { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = default!;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        
        // Populate roles excluding complex logic if needed, here simply listing all available
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        RoleOptions = new SelectList(roles);
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        
        // Repopulate roles in case of return due to error
        var rolesList = _roleManager.Roles.Select(r => r.Name).ToList();
        RoleOptions = new SelectList(rolesList);

        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Rut = Input.Rut,
                Dv = Input.Dv,
                NombreCompleto = Input.NombreCompleto,
                Cargo = Input.Cargo
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Assign Role
                if (!string.IsNullOrEmpty(Input.Role))
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);
                }

                // Auto-login after registration
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}