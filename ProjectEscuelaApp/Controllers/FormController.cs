using Microsoft.AspNetCore.Mvc;
using WebEscuelaProject.Models;
using WebEscuelaProject.Logic;
using WebEscuelaProject.Models.Filters;
using WebEscuelaProject.Services;
using System.Security.Principal;

namespace WebEscuelaProject.Controllers
{
    [NotLoggedFilter]
    public class FormController : Controller
    {
        private IConfiguration _config;
        Form form = new Form();
        private readonly IEmailService _emailService;

        public FormController(IConfiguration config, IEmailService emailService)
        {
            _config = config;
            _emailService = emailService;
        }


        public ActionResult InabilityForm()
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

        [HttpPost]
        public ActionResult SubmitInabilityForm(InabilityFormModel formObj)
        {
            try
            {
                form.SubmitInabilityForm(_config, formObj);
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

        [HttpGet]
        public ActionResult PermissionForm()
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

        [HttpPost]
        public ActionResult SubmitPermissionForm(PermissionFormModel formObj)
        {
            try
            {
                form.SubmitPermissionForm(_config, formObj, _emailService, GenerateURL());
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

        private string GenerateURL()
        {
            //Se crea el token en el controlador para poder utilizar la clase Request y armar el callbackURL
            return Request.Scheme + "://" + Request.Host + Url.Action("PermissionFileList", "FileManager");
        }
    }
}
