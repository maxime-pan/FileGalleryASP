using FileGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FileGallery.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AnonymousUploadsController : Controller
{
    private readonly IFileRepository _repo;
    private readonly IWebHostEnvironment _env;
    private readonly ISettingsService _settings;
    public AnonymousUploadsController(IFileRepository repo, IWebHostEnvironment env, ISettingsService settings)
    { _repo = repo; _env = env; _settings = settings; }

    public async Task<IActionResult> Index()
    {
        var uploads = await _repo.GetAnonymousUploadsAsync();
        return View(uploads);
    }

    [HttpGet]
    public IActionResult Download(string storedFileName)
    {
        var storageRoot = _settings.Get("StorageRoot", "Content");
        var anonFolderName = _settings.Get("AnonymousUploadFolder", "publicupload");
        var fullPath = System.IO.Path.Combine(_env.ContentRootPath, storageRoot, anonFolderName, storedFileName);
        if (!System.IO.File.Exists(fullPath)) return NotFound();
        var provider = new FileExtensionContentTypeProvider();
        var contentType = provider.TryGetContentType(fullPath, out var ct) ? ct : "application/octet-stream";
        return PhysicalFile(fullPath, contentType, storedFileName);
    }
}
