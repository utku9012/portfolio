using portfolio.Models.ViewModels;

namespace portfolio.Services;

public interface IPortfolioService
{
    Task<PortfolioViewModel> GetPortfolioAsync();
}
