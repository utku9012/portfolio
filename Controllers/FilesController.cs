using Microsoft.AspNetCore.Mvc;
using portfolio.Data;

namespace portfolio.Controllers;

[Route("files")]
public class FilesController : Controller
{
    private readonly ApplicationDbContext _context;

    public FilesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var asset = await _context.UploadedAssets.FindAsync(id);
        if (asset is null)
        {
            return NotFound();
        }

        return File(asset.Data, asset.ContentType, asset.FileName);
    }
}
