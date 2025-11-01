using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotnetAPI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        ViewData["GrafanaIframeUrl"] = GetUrl();
    }
    public string GetUrl()
    {
        DotNetEnv.Env.Load();
        string? dashboard = DotNetEnv.Env.GetString("GF_DASHBOARD");

        if (dashboard is not null)
        {
            return dashboard.ToString();
        }

        return "NO URL";
    }
}
