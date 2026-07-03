using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using portfolio.Models;
using portfolio.Services;

namespace portfolio.Controllers;

public class HomeController : Controller
{
    private readonly IPortfolioService _portfolioService;

    public HomeController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _portfolioService.GetPortfolioAsync());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = 500,
            Title = "Something went wrong",
            Message = "The site had trouble handling this request. Please try again in a moment."
        });
    }

    [Route("/Home/StatusCodePage")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult StatusCodePage(int code)
    {
        var model = code == 404
            ? new ErrorViewModel
            {
                StatusCode = 404,
                Title = "Page not found",
                Message = "The page you are looking for does not exist or may have been moved."
            }
            : new ErrorViewModel
            {
                StatusCode = code,
                Title = "Request could not be completed",
                Message = "The site could not complete this request. Please return to the portfolio and try again."
            };

        model.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        Response.StatusCode = code;
        return View("Error", model);
    }
}
