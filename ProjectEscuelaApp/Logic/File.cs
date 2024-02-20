using Dapper;
using System.Data.SQLite;
using WebEscuelaProject.Models;

namespace WebEscuelaProject.Logic
{
    public class File
    {
        public async Task UploadPublicFile(PublicFileModel fileObj, IConfiguration _config)
        {
            if (fileObj.file != null)
            {
                var file = fileObj.file;
                var file_extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                string[] validExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".rtf", ".odt" };


                if (validExtensions.Contains(file_extension))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
                        {
                            var localTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

                            var filename = file.FileName;
                            var filecontent = memoryStream.ToArray();
                            var filedate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimezone);
                            var filetype = fileObj.filetype;
                            var file_visualName = fileObj.file_visualName;

                            var parameters = new
                            {
                                filename,
                                filedate,
                                filecontent,
                                filetype,
                                file_visualName,
                            };

                            var query = "INSERT INTO public_files (filename, filedate, filecontent, filetype, file_visualName) " +
                                "VALUES (@filename, @filedate, @filecontent, @filetype, @file_visualName)";

                            context.Execute(query, parameters);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("No se permite este tipo de archivo");
                }
            }
        }
        public async Task UploadPlan(PlanFileModel fileObj, IConfiguration _config, int userid)
        {
            if (fileObj.file != null)
            {
                var file = fileObj.file;
                var file_extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                string[] validExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".rtf", ".odt" };

                if (validExtensions.Contains(file_extension))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
                        {
                            var localTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

                            var plan_name = fileObj.plan_name;
                            var plan_subject = fileObj.plan_subject;
                            var plan_date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimezone);
                            var plan_filename = file.FileName;
                            var plan_filecontent = memoryStream.ToArray();

                            var parameters = new
                            {
                                plan_name,
                                plan_subject,
                                plan_date,
                                plan_filename,
                                plan_filecontent,
                                userid
                            };

                            var query = "INSERT INTO plan_files (plan_name, plan_subject, plan_date, plan_filename, plan_filecontent, user_id) " +
                                "VALUES (@plan_name, @plan_subject, @plan_date, @plan_filename, @plan_filecontent, @userid)";

                            context.Execute(query, parameters);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("No se permite este tipo de archivo");
                }
            }
        }
        public async Task UploadMiscFile(MiscFileModel fileObj, IConfiguration _config, int userid)
        {
            if (fileObj.file != null)
            {
                var file = fileObj.file;
                var file_extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                string[] validExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".rtf", ".odt" };

                if (validExtensions.Contains(file_extension))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
                        {
                            var localTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

                            var filename = file.FileName;
                            var filecontent = memoryStream.ToArray();
                            var filedate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimezone);
                            var file_visualName = fileObj.file_visualName;
                            var filetype = fileObj.filetype;

                            var parameters = new
                            {
                                filename,
                                filedate,
                                filecontent,
                                filetype,
                                file_visualName,
                                userid
                            };

                            var query = "INSERT INTO misc_files (filename, filedate, filecontent, filetype, file_visualName, user_id) " +
                                "VALUES (@filename, @filedate, @filecontent, @filetype, @file_visualName, @userid)";

                            context.Execute(query, parameters);
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("No se permite este tipo de archivo");
                }
            }

        }
    }
}

