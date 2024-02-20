using WebEscuelaProject.Logic;
using WebEscuelaProject.Models;
using Microsoft.AspNetCore.Mvc;
using WebEscuelaProject.Models.Filters;
using iText.StyledXmlParser.Jsoup.Parser;

namespace WebEscuelaProject.Controllers
{
    public class AccountController : Controller
    {

        Account account = new Account();
        private IConfiguration _config;
        private readonly IEmailService _emailService;

        public AccountController(IConfiguration config, IEmailService emailService)
        {
            _config = config;
            _emailService = emailService;
        }

        #region Autentificación 

        [HttpGet, AlreadyLoggedFilter]
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpPost, AlreadyLoggedFilter]
        public ActionResult Login(AccountModel user)
        {
            try
            {
                //Evitar datos null
                if (user.user_username == null || user.user_password == null)
                {
                    user.user_username = "";
                    user.user_password = "";
                }

                user.user_username = user.user_username.Trim().ToLower();
                var response = account.Login(_config, user);


                // Nombre de usuario no vacío = Éxito en el Login. 
                if (!string.IsNullOrEmpty(response.user_username))
                {
                    HttpContext.Session.SetString("id", response.user_id.ToString());
                    HttpContext.Session.SetString("username", response.user_username);
                    HttpContext.Session.SetString("usermail", response.user_email);
                    HttpContext.Session.SetString("user_role", response.user_role);
                    HttpContext.Session.SetString("user_firstname", response.user_firstname);
                    HttpContext.Session.SetString("user_lastname1", response.user_lastname1);
                    HttpContext.Session.SetString("user_lastname2", response.user_lastname2);

                    return RedirectToAction("Index", "Home");
                }

                //Si falla el login, response envía la razón (contaseña o usuario inválidos)
                return View(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        public ActionResult LogOff()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        #endregion

        #region Gestion Admin

        [HttpGet, NotAuthorizedFilter]
        public ActionResult CreateAccount()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpPost, NotAuthorizedFilter]
        public ActionResult CreateAccount(AccountModel user)
        {
            try
            {
                //Evitar datos null
                if (string.IsNullOrEmpty(user.user_email) || string.IsNullOrEmpty(user.user_firstname) ||
                    string.IsNullOrEmpty(user.user_lastname1) || string.IsNullOrEmpty(user.user_lastname2) || string.IsNullOrEmpty(user.user_role))
                {
                    throw new InvalidOperationException("No se pueden envíar datos vacíos");
                }

                user.user_email = user.user_email.Trim();
                user.user_resetToken = account.GenerateRandomToken();
                return View(account.CreateAccount(_config, _emailService, user, GenerateURL(user.user_resetToken)));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }


        [HttpGet, NotAuthorizedFilter]
        public ActionResult UserList()
        {
            try
            {
                return View(account.GetUserList(_config));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult ToggleAccount(int id)
        {
            try
            {
                account.ToggleAccount(_config, id);
                return RedirectToAction("UserList", "Account");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpGet, NotAuthorizedFilter]
        public ActionResult AdminResetPassword(int id)
        {
            try
            {
                return View(account.GetUserByID(_config, id));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpPost, NotAuthorizedFilter]
        public ActionResult AdminResetPassword(AccountModel user)
        {
            try
            {
                //Evitar datos null
                if (string.IsNullOrEmpty(user.user_email) || string.IsNullOrEmpty(user.user_password))
                {
                    throw new InvalidOperationException("No se pueden envíar datos vacíos");
                }

                account.ResetPassword(_config, user);
                user.ResponseMessage = "PasswordReset";
                return View(user);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        //Función Utilizada por el AJAX
        [HttpDelete, NotAuthorizedFilter]
        public ActionResult DeleteAccount(int id)
        {
            try
            {
                account.DeleteAccount(_config, id);
                return Ok(new { Msg = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMsg = $"Error: {ex}" });
            }
        }

        #endregion

        #region Restablecer Contraseña

        [HttpGet, AlreadyLoggedFilter]
        public ActionResult RequestPasswordReset()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpPost, AlreadyLoggedFilter]
        public ActionResult RequestPasswordReset(AccountModel user)
        {
            try
            {
                //Evitar datos null
                if (string.IsNullOrEmpty(user.user_email))
                {
                    throw new InvalidOperationException("No se pueden envíar datos vacíos");
                }

                user.user_email = user.user_email.Trim();
                user.user_resetToken = account.GenerateRandomToken();
                account.RequestPasswordReset(_config, _emailService, user, GenerateURL(user.user_resetToken));
                user.ResponseMessage = "Success";

                return View(user);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        [HttpGet, AlreadyLoggedFilter]
        public ActionResult ResetPassword(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Index", "Home");
                }

                var response = account.CheckToken(_config, token);

                if (response.ResponseMessage != "WrongToken") HttpContext.Session.Clear();

                return View(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }


        [HttpPost, AlreadyLoggedFilter]
        public ActionResult ResetPassword(AccountModel user)
        {
            try
            {
                //Evitar datos null
                if (string.IsNullOrEmpty(user.user_email) || string.IsNullOrEmpty(user.user_password))
                {
                    throw new InvalidOperationException("No se pueden envíar datos vacíos");
                }

                account.ResetPassword(_config, user);
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View("Error");
            }
        }

        #endregion

        #region Funciones Auxiliares
        private string GenerateURL(string token)
        {
            //Se crea el token en el controlador para poder utilizar la clase Request y armar el callbackURL
            return Request.Scheme + "://" + Request.Host + Url.Action("ResetPassword", "Account", new { token });
        }

        private void HandleException(Exception ex)
        {
            ViewBag.Error = ex.Message;
            HttpContext.Session.SetString("Error", "true");
        }
    }
}

#endregion
