using FileGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileGallery.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class DashboardController : Controller
{
    private readonly IFileRepository _repo;
    public DashboardController(IFileRepository repo) => _repo = repo;
    public async Task<IActionResult> Index()
    {
        var cats = await _repo.GetCategoriesAsync();
        var filesCount = cats.Sum(c => c.Files?.Count ?? 0);
        var uploads = await _repo.GetAnonymousUploadsAsync();
        ViewBag.CategoriesCount = cats.Count;
        ViewBag.FilesCount = filesCount;
        ViewBag.AnonymousCount = uploads.Count;
        return View();
    }
}
