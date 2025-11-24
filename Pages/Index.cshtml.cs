using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotnetAPI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
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

        _logger.Log(LogLevel.Error, "There was no url in env.");
        return "";
    }
}
