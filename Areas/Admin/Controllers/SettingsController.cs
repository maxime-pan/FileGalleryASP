using FileGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileGallery.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SettingsController : Controller
{
    private readonly ISettingsService _settings;
    public SettingsController(ISettingsService settings) => _settings = settings;

    public IActionResult Index()
    {
        ViewBag.RequireWindowsAuth = _settings.GetBool("RequireWindowsAuth", false);
        ViewBag.AllowedExtensions = _settings.Get("AllowedExtensions", ".pdf,.png,.jpg,.jpeg,.gif,.txt,.csv,.mp4,.mp3");
        ViewBag.MaxUploadSizeMB = _settings.GetInt("MaxUploadSizeMB", 50);
        ViewBag.AdminADGroup = _settings.Get("AdminADGroup", "");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(bool requireWindowsAuth, string allowedExtensions, int maxUploadSizeMB, string adminADGroup)
    {
        await _settings.SetAsync("RequireWindowsAuth", requireWindowsAuth.ToString());
        await _settings.SetAsync("AllowedExtensions", allowedExtensions);
        await _settings.SetAsync("MaxUploadSizeMB", maxUploadSizeMB.ToString());
        await _settings.SetAsync("AdminADGroup", adminADGroup);
        TempData["Message"] = "Settings saved.";
        return RedirectToAction("Index");
    }
}
