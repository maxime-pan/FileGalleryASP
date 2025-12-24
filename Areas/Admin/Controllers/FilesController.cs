using FileGallery.Models;
using FileGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileGallery.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FilesController : Controller
{
    private readonly IFileRepository _repo;
    private readonly IWebHostEnvironment _env;
    private readonly ISettingsService _settings;
    public FilesController(IFileRepository repo, IWebHostEnvironment env, ISettingsService settings)
    { _repo = repo; _env = env; _settings = settings; }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var cats = await _repo.GetCategoriesAsync();
        var selected = categoryId ?? cats.FirstOrDefault()?.Id;
        var files = selected.HasValue ? await _repo.GetFilesByCategoryAsync(selected.Value, onlyVisible: false) : new List<FileItem>();
        ViewBag.Categories = cats;
        ViewBag.SelectedCategoryId = selected;
        return View(files);
    }

    public async Task<IActionResult> Edit(int? id, int categoryId)
    {
        var f = id.HasValue ? await _repo.GetFileAsync(id.Value) : new FileItem{ CategoryId = categoryId };
        return View(f);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FileItem file)
    {
        await _repo.AddOrUpdateFileAsync(file);
        return RedirectToAction("Index", new { categoryId = file.CategoryId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int categoryId)
    {
        await _repo.DeleteFileAsync(id);
        return RedirectToAction("Index", new { categoryId });
    }
}
