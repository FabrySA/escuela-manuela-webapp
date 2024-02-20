using Microsoft.AspNetCore.Mvc;
using WebEscuelaProject.Logic;
using WebEscuelaProject.Models.Filters;

namespace WebEscuelaProject.Controllers
{
    public class FileManagerController : Controller
    {
        private IConfiguration _config;
        FileManager fileManager = new FileManager();

        public FileManagerController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult InabilityFileList()
        {
            try
            {
                return View(fileManager.GetInabilityFileList(_config));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult PermissionFileList()
        {
            try
            {
                return View(fileManager.GetPermissionFileList(_config));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult UsersPlanList()
        {
            try
            {
                return View(fileManager.GetPlanUserList(_config));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult PlanFileList(int id)
        {
            try
            {
                if (id != 0)
                {
                    var files = fileManager.GetPlanFileList(id, _config);

                    if (files.FirstOrDefault() != null)
                    {
                        return View(files);
                    }
                    return RedirectToAction("Index", "Home");
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

        [HttpGet, NotAuthorizedFilter]
        public ActionResult UsersMiscList()
        {
            try
            {
                return View(fileManager.GetMiscUserList(_config));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult MiscFileList(int id)
        {
            try
            {
                if (id != 0)
                {
                    var files = fileManager.GetMiscFileList(id, _config);

                    if (files.FirstOrDefault() != null)
                    {
                        return View(files);
                    }
                    return RedirectToAction("Index", "Home");
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

        [HttpGet, NotLoggedFilter]
        public ActionResult TemplateFileList()
        {
            try
            {
                return View(fileManager.GetPublicFileList("template", _config, false));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [HttpGet, NotLoggedFilter]
        public ActionResult NewsletterFileList()
        {
            try
            {
                return View(fileManager.GetPublicFileList("newsletter", _config, false));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult AdminPublicFileList()
        {
            try
            {
                return View(fileManager.GetPublicFileList("", _config, true));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                HttpContext.Session.SetString("Error", "true");
                return View("Error");
            }
        }


        //DELETE

        [HttpDelete, NotAuthorizedFilter]
        public ActionResult DeleteFile(string type, int id, string consec)
        {
            try
            {
                fileManager.DeleteFile(type, id, consec, _config);
                return Ok(new { Msg = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMsg = $"Error: {ex}" });
            }
        }

    }
}
