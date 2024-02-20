using Dapper;
using System.Data.SQLite;
using WebEscuelaProject.Models;

namespace WebEscuelaProject.Logic
{
    public class FileManager
    {
        public List<FormFileModel> GetInabilityFileList(IConfiguration _config)
        {
            var query = "SELECT * FROM 'inability_file'";

            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var files = contexto.Query<FormFileModel>(query).ToList();

                if (files != null)
                {
                    return files;
                }
                return new List<FormFileModel>();
            }
        }

        public List<FormFileModel> GetPermissionFileList(IConfiguration _config)
        {
            var query = "SELECT * FROM 'permission_file'";

            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var files = contexto.Query<FormFileModel>(query).ToList();

                if (files != null)
                {
                    return files;
                }
                return new List<FormFileModel>();
            }
        }

        public List<PublicFileModel> GetPublicFileList(string type, IConfiguration _config, bool isAdmin)
        {
            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var query = "";

                if (isAdmin)
                {
                    query = "SELECT * FROM public_files";
                }
                else
                {
                    query = "SELECT * FROM public_files WHERE filetype = @type";
                }

                var files = contexto.Query<PublicFileModel>(query, new { type }).ToList();

                if (files != null)
                {
                    foreach (var item in files)
                    {
                        item.filextension = Path.GetExtension(item.filename).TrimStart('.');
                    }
                    return files;
                }
                return new List<PublicFileModel>();
            }
        }

        public List<AccountModel> GetPlanUserList(IConfiguration _config)
        {
            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var query = "SELECT user_id, user_firstname, user_lastname1, user_lastname2 " +
                    "FROM user_account WHERE EXISTS (SELECT 1 FROM plan_files WHERE plan_files.user_id = user_account.user_id)";

                var files = contexto.Query<AccountModel>(query).ToList();

                if (files != null)
                {
                    return files;
                }
                return new List<AccountModel>();
            }
        }

        public List<PlanFileModel> GetPlanFileList(int userid, IConfiguration _config)
        {
            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var query = "SELECT p.*, u.user_firstname, u.user_lastname1, u.user_lastname2 FROM plan_files p " +
                    "INNER JOIN user_account u ON u.user_id = p.user_id WHERE p.user_id = @userid";

                var files = contexto.Query<PlanFileModel>(query, new { userid }).ToList();

                if (files != null)
                {
                    foreach (var item in files)
                    {
                        item.plan_filextension = Path.GetExtension(item.plan_filename).TrimStart('.');
                    }
                    return files;
                }
                return new List<PlanFileModel>();
            }
        }

        public List<AccountModel> GetMiscUserList(IConfiguration _config)
        {
            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var query = "SELECT user_id, user_firstname, user_lastname1, user_lastname2 " +
                    "FROM user_account WHERE EXISTS (SELECT 1 FROM misc_files WHERE misc_files.user_id = user_account.user_id)";

                var files = contexto.Query<AccountModel>(query).ToList();

                if (files != null)
                {
                    return files;
                }
                return new List<AccountModel>();
            }
        }

        public List<MiscFileModel> GetMiscFileList(int userid, IConfiguration _config)
        {
            using (var contexto = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var query = "SELECT p.*, u.user_firstname, u.user_lastname1, u.user_lastname2 FROM misc_files p " +
                    "INNER JOIN user_account u ON u.user_id = p.user_id WHERE p.user_id = @userid";

                var files = contexto.Query<MiscFileModel>(query, new { userid }).ToList();

                if (files != null)
                {
                    foreach (var item in files)
                    {
                        item.filextension = Path.GetExtension(item.filename).TrimStart('.');
                    }
                    return files;
                }
                return new List<MiscFileModel>();
            }
        }


        //GENERIC DELETE
        public void DeleteFile(string type, int id, string consec, IConfiguration _config)
        {
            var query = "";

            switch (type)
            {
                case "inability":
                    query = $"DELETE FROM 'inability_file' WHERE file_id = {id}";
                    break;
                case "permission":
                    query = $"DELETE FROM 'permission_file' WHERE file_id = {id}";
                    break;
                case "public":
                    query = $"DELETE FROM 'public_files' WHERE file_id = {id}";
                    break;
                case "plan":
                    query = $"DELETE FROM 'plan_files' WHERE id_plan = {id}";
                    break;
                case "misc":
                    query = $"DELETE FROM 'misc_files' WHERE file_id = {id}";
                    break;
            }

            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                if (type.Equals("permission") && !string.IsNullOrEmpty(consec))
                {
                    var query2 = $"DELETE FROM consecutive_ids WHERE consecutive = '{consec}'";
                    context.Execute(query2);
                }

                context.Execute(query);
            }
        }
    }
}
