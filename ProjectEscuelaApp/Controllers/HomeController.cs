using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebEscuelaProject.Models;
using WebEscuelaProject.Models.Filters;


namespace WebEscuelaProject.Controllers
{
    public class HomeController : Controller
    {
        [NotLoggedFilter]
        public IActionResult Index()
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

        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue && statusCode.Value == 404)
            {
                return View("Error404");
            }

            if (HttpContext.Session.GetString("Error") == "true")
            {
                HttpContext.Session.Remove("Error");
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [NotLoggedFilter]
        public ActionResult Confirmation()
        {
            try
            {
                if (HttpContext.Session.GetString("UploadSuccessful") == "true")
                {
                    HttpContext.Session.Remove("UploadSuccessful");
                    return View();
                }
                return RedirectToAction("Index", "Home");
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