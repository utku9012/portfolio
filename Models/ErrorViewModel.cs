namespace portfolio.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public int StatusCode { get; set; } = 500;

    public string Title { get; set; } = "Something went wrong";

    public string Message { get; set; } = "The page could not be loaded right now. Please try again later.";

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
