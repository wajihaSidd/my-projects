using Learning_Management_System.Models.ENTITIES;
using Microsoft.AspNetCore.Mvc;

namespace Learning_Management_System.Controllers
{
    public class AdminSettingsController : Controller
    {
        private readonly DatabaseContext _context;

        public AdminSettingsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /AdminSettings/Index
        public IActionResult Index()
        {
            var settings = _context.SystemSettings.FirstOrDefault() ?? new SystemSettings();
            return View(settings);
        }

        // POST: /AdminSettings/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SystemSettings model)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.SystemSettings.FirstOrDefault();
                if (existing != null)
                {
                    // Update existing
                    existing.SiteName = model.SiteName;
                    existing.LogoUrl = model.LogoUrl;
                    existing.FaviconUrl = model.FaviconUrl;
                    existing.DefaultLanguage = model.DefaultLanguage;
                    existing.Timezone = model.Timezone;
                    existing.ContactEmail = model.ContactEmail;
                    existing.FooterText = model.FooterText;
                    existing.EnableRegistration = model.EnableRegistration;
                    existing.MaintenanceMode = model.MaintenanceMode;
                }
                else
                {
                    // Insert new
                    _context.SystemSettings.Add(model);
                }

                _context.SaveChanges();
                TempData["Success"] = "Settings updated successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
