using FileGallery.Models;
using FileGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileGallery.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CategoriesController : Controller
{
    private readonly IFileRepository _repo;
    private readonly IWebHostEnvironment _env;
    private readonly ISettingsService _settings;
    public CategoriesController(IFileRepository repo, IWebHostEnvironment env, ISettingsService settings)
    { _repo = repo; _env = env; _settings = settings; }

    public async Task<IActionResult> Index()
    {
        return View(await _repo.GetCategoriesAsync());
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return View(new Category());
        var cat = await _repo.GetCategoryAsync(id.Value);
        return View(cat);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Category cat)
    {
        await _repo.AddOrUpdateCategoryAsync(cat);
        var storageRoot = _settings.Get("StorageRoot", "Content");
        var dir = System.IO.Path.Combine(_env.ContentRootPath, storageRoot, cat.FolderPath ?? "");
        Directory.CreateDirectory(dir);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteCategoryAsync(id);
        return RedirectToAction("Index");
    }
}
