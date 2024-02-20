using Microsoft.AspNetCore.Mvc;
using WebEscuelaProject.Models;
using WebEscuelaProject.Models.Filters;

namespace WebEscuelaProject.Controllers
{
    public class FileController : Controller
    {
        Logic.File file = new Logic.File();
        private IConfiguration _config;

        public FileController(IConfiguration config)
        {
            _config = config;
        }

        [NotAuthorizedFilter]
        public ActionResult UploadPublicFile()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [NotAuthorizedFilter, HttpPost]
        public async Task<IActionResult> UploadPublicFile(PublicFileModel fileObj)
        {
            try
            {
                await file.UploadPublicFile(fileObj, _config);
                HttpContext.Session.SetString("UploadSuccessful", "true");
                return RedirectToAction("Confirmation", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }


        [NotLoggedFilter]
        public ActionResult UploadPlan()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [NotLoggedFilter, HttpPost]
        public async Task<IActionResult> UploadPlan(PlanFileModel fileObj)
        {
            try
            {
                await file.UploadPlan(fileObj, _config, int.Parse(HttpContext.Session.GetString("id")));
                HttpContext.Session.SetString("UploadSuccessful", "true");
                return RedirectToAction("Confirmation", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [NotLoggedFilter]
        public ActionResult UploadMiscFile()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [NotLoggedFilter, HttpPost]
        public async Task<IActionResult> UploadMiscFile(MiscFileModel fileObj)
        {
            try
            {
                await file.UploadMiscFile(fileObj, _config, int.Parse(HttpContext.Session.GetString("id")));
                HttpContext.Session.SetString("UploadSuccessful", "true");
                return RedirectToAction("Confirmation", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }
    }
}
