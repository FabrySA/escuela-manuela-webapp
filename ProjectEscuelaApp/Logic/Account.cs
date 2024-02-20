using Dapper;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using WebEscuelaProject.Models;


namespace WebEscuelaProject.Logic
{
    public class Account
    {

        public AccountModel Login(IConfiguration _config, AccountModel user)
        {
            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                string query = "SELECT user_id, user_username, user_password, user_firstname, user_lastname1, " +
                    "user_lastname2, user_email, user_role, user_resetToken, user_resetTokenExpires, CAST(user_isActive AS BOOLEAN) " +
                    "AS user_isActive FROM 'user_account' WHERE user_username=@user_username;";

                var response = context.QueryFirstOrDefault<AccountModel>(query, new { user_username = user.user_username });

                if (response != null)
                {
                    if (!response.user_isActive)
                    {
                        user.ResponseMessage = "NotActive";
                        user.user_username = "";
                        return user;
                    }

                    if (BCrypt.Net.BCrypt.Verify(user.user_password, response.user_password))
                    {
                        return response;
                    }
                    user.ResponseMessage = "WrongPass";
                    user.user_username = "";
                    return user;
                }
                user.ResponseMessage = "WrongUser";
                user.user_username = "";
                return user;
            }
        }
        public AccountModel CreateAccount(IConfiguration _config, IEmailService _emailService, AccountModel user, string callbackURL)
        {

            var mailIndex = user.user_email.IndexOf('@');
            user.user_username = user.user_email.Substring(0, mailIndex);
            user.user_resetTokenExpires = DateTime.UtcNow.AddDays(1);
            user.user_isActive = false;
            user.user_password = GeneratePasswordHash(GenerateRandomPass());

            string queryVerify = "SELECT COUNT(*) FROM 'user_account' WHERE user_email = @user_email";
            string queryInsert = "INSERT INTO 'user_account'(user_username, user_password, user_email, user_role, user_firstname, user_lastname1, user_lastname2, " +
                        "user_resetToken, user_resetTokenExpires, user_isActive) " +
                        "VALUES (@user_username, @user_password, @user_email, @user_role, @user_firstname, @user_lastname1, @user_lastname2, " +
                        "@user_resetToken, @user_resetTokenExpires, @user_isActive)";


            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {

                var existingUser = context.QueryFirstOrDefault<int>(queryVerify, new { user.user_email });

                if (existingUser == 0)
                {
                    context.Execute(queryInsert, new
                    {
                        user.user_username,
                        user.user_password,
                        user.user_email,
                        user.user_role,
                        user.user_firstname,
                        user.user_lastname1,
                        user.user_lastname2,
                        user.user_resetToken,
                        user.user_resetTokenExpires,
                        user.user_isActive
                    });

                    EmailDto emailinfo = new EmailDto();
                    emailinfo.Subject = "Creación de un Nuevo Usuario";
                    emailinfo.To = user.user_email;
                    emailinfo.Body = "<h1>Escuela Manuela Santamaría Rodríguez</h1> <p>Estimado usuario,</p> <p>Le informamos que se ha creado una cuenta " +
                    "en nuestro sistema a su nombre. Para proceder con la generación de contraseña, le solicitamos acceder a " +
                    $"<strong><a href='{callbackURL}'>este enlace</a>.</strong> Tenga en cuenta que dispone de un plazo de 24 horas " +
                    $"antes de que la solicitud expire.</p> <p>Tras generar una nueva contraseña, puede iniciar sesión con el usuario: <strong>{user.user_username}</strong> " +
                    "</p> <p>Si no ha solicitado este correo, le pedimos que ignore el mensaje.</p>";
                    _emailService.SendEmail(emailinfo);
                }
                else
                {
                    user.ResponseMessage = "Duplicated";
                    return user;
                }
            }
            user.ResponseMessage = "Success";
            return user;
        }

        public void RequestPasswordReset(IConfiguration _config, IEmailService _emailService, AccountModel user, string callbackURL)
        {
            user.user_resetTokenExpires = DateTime.UtcNow.AddDays(1);

            int startIndex = callbackURL.LastIndexOf('=') + 1;
            user.user_resetToken = callbackURL.Substring(startIndex);

            var query = "UPDATE 'user_account' SET user_resetToken = @user_resetToken, user_resetTokenExpires = @user_resetTokenExpires WHERE user_email = @user_email";

            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var response = context.Execute(query, new { user.user_email, user.user_resetToken, user.user_resetTokenExpires });

                if (response != 0)
                {
                    EmailDto emailinfo = new EmailDto();
                    emailinfo.Subject = "Solicitud para Restablecer su Contraseña";
                    emailinfo.To = user.user_email;
                    emailinfo.Body = "<h1>Escuela Manuela Santamaría Rodríguez</h1> <p>Estimado usuario,</p> <p>Ha solicitado restablecer su contraseña. " +
                       $"Para continuar con el proceso, le invitamos a acceder a <strong><a href='{callbackURL}'>este enlace</a>.</strong> " +
                       "Tenga en cuenta que dispone de un plazo de 24 horas antes de que la solicitud expire.</p> <p>Si no ha solicitado este correo, " +
                       "le pedimos que ignore el mensaje.</p>";

                    _emailService.SendEmail(emailinfo);
                }
            }
        }

        public AccountModel CheckToken(IConfiguration _config, string userToken)
        {
            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                AccountModel obj = new AccountModel();

                var query = "SELECT user_email, user_resetTokenExpires, user_isActive FROM 'user_account' WHERE user_resetToken = @userToken LIMIT 1";

                var response = context.Query<AccountModel>(query, new { userToken }).FirstOrDefault();

                if (response == null)
                {
                    obj.ResponseMessage = "WrongToken";
                    return obj;
                }

                if (response.user_resetTokenExpires < DateTime.UtcNow)
                {
                    obj.ResponseMessage = "TokenExpired";
                    return obj;
                }

                return response;
            }
        }

        public AccountModel GetUserByID(IConfiguration _config, int user_id)
        {
            var query = "SELECT user_email FROM 'user_account' WHERE user_id = @user_id";

            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var response = context.Query<AccountModel>(query, new { user_id }).FirstOrDefault();

                if (response != null)
                {
                    return response;
                }

                AccountModel user = new AccountModel();
                user.ResponseMessage = "UserNotFound";
                return user;
            }
        }

        public void ResetPassword(IConfiguration _config, AccountModel user)
        {
            var new_pass = user.user_password;

            var query = "UPDATE 'user_account' SET user_password = @user_password, user_isActive = 1, user_resetToken = '' WHERE user_email = @user_email";

            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                user.user_password = GeneratePasswordHash(new_pass);
                context.Execute(query, new { user.user_email, user.user_password });
            }

            user.user_password = new_pass;
        }

        public List<AccountModel> GetUserList(IConfiguration _config)
        {
            var query = "SELECT user_id, user_firstname, user_lastname1, user_lastname2, user_isActive, user_email, user_role FROM user_account";

            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var userList = contexto.Query<AccountModel>(query).ToList();

                if (userList != null)
                {
                    return userList;
                }
                return new List<AccountModel>();
            }
        }

        public void ToggleAccount(IConfiguration _config, int user_id)
        {
            var query = "UPDATE 'user_account' SET user_isActive = CASE WHEN user_isActive = 1 THEN 0 ELSE 1 END WHERE user_id = @user_id";

            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var files = contexto.Query<AccountModel>(query, new { user_id }).ToList();
            }
        }

        public void DeleteAccount(IConfiguration _config, int user_id)
        {
            var query = "DELETE FROM 'user_account' WHERE user_id = @user_id";

            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                contexto.Execute(query, new { user_id });
            }
        }

        private string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private string GenerateRandomPass()
        {
            const int length = 12;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@$?_-";

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(allowedChars.Length);
                password.Append(allowedChars[randomIndex]);
            }


            return password.ToString();
        }

        public string GenerateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}


