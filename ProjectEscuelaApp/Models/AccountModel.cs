using System.ComponentModel.DataAnnotations.Schema;

namespace WebEscuelaProject.Models
{
    public class AccountModel
    {
        public int user_id { get; set; }
        public string user_username { get; set; } = String.Empty;
        public string user_password { get; set; } = String.Empty;
        public string user_firstname { get; set; } = String.Empty;
        public string user_lastname1 { get; set; } = String.Empty;
        public string user_lastname2 { get; set; } = String.Empty;
        public string user_email { get; set; } = String.Empty;
        public string user_role { get; set; } = String.Empty;
        public string user_resetToken { get; set; } = String.Empty;
        public DateTime user_resetTokenExpires { get; set; } = DateTime.UtcNow;
        public bool user_isActive { get; set; } = false;

        [NotMapped]
        public string ResponseMessage { get; set; } = String.Empty;
    }
}
