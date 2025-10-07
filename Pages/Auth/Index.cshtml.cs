using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DotnetAPI.Pages.Auth;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public string UserNameOrEmail { get; set; }
    [BindProperty]
    public string Password { get; set; }
    public string? AuthMessage { get; set; }

    [BindProperty]
    public string RegisterUserName { get; set; }
    [BindProperty]
    public string RegisterEmail { get; set; }
    [BindProperty]
    public string RegisterPassword { get; set; }
    public string? RegisterMessage { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var loginData = new
        {
            UserNameOrEmail,
            Password
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginData),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync("http://localhost:5230/auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            AuthMessage = "Login exitoso: " + json;
        }
        else
        {
            AuthMessage = "Login fallido";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostRegisterAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var registerData = new
        {
            UserName = RegisterUserName,
            Email = RegisterEmail,
            Password = RegisterPassword
        };

        var content = new StringContent(
            JsonSerializer.Serialize(registerData),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync("http://localhost:5230/auth/register", content);

        if (response.IsSuccessStatusCode)
        {
            RegisterMessage = "Registro exitoso. Ahora puedes iniciar sesión.";
        }
        else
        {
            RegisterMessage = "Error al registrar usuario. Verifica los datos.";
        }

        return Page();
    }
}