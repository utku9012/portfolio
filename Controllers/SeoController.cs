using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace portfolio.Controllers;

public class SeoController : Controller
{
    [HttpGet("/robots.txt")]
    public IActionResult Robots()
    {
        var baseUrl = GetBaseUrl();
        var robots = $"""
            User-agent: *
            Disallow: /Admin
            Disallow: /Admin/

            Sitemap: {baseUrl}/sitemap.xml
            """;

        return Content(robots, "text/plain", Encoding.UTF8);
    }

    [HttpGet("/sitemap.xml")]
    public IActionResult Sitemap()
    {
        var baseUrl = GetBaseUrl();
        var sitemap = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
              <url>
                <loc>{baseUrl}/</loc>
                <changefreq>weekly</changefreq>
                <priority>1.0</priority>
              </url>
            </urlset>
            """;

        return Content(sitemap, "application/xml", Encoding.UTF8);
    }

    private string GetBaseUrl()
    {
        return $"{Request.Scheme}://{Request.Host}".TrimEnd('/');
    }
}
