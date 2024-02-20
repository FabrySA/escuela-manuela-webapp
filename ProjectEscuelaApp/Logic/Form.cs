using Dapper;
using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Data.SQLite;
using WebEscuelaProject.Models;

namespace WebEscuelaProject.Logic
{
    public class Form
    {
        public void SubmitInabilityForm(IConfiguration _config, InabilityFormModel formData)
        {
            var pdfPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Resources", "inability_form_template.pdf");
            var filename = $"boleta_incapacidad_{formData.name + "_" + formData.lastname1}.pdf";

            if (!String.IsNullOrWhiteSpace(formData.signature))
            {
                formData.signature = formData.signature.Split(",")[1];
            }

            var localTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");
            var localDateTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimezone);

            var name = formData.name;
            var lastname1 = formData.lastname1;
            var lastname2 = formData.lastname2;
            var fullName = name + " " + lastname1 + " " + lastname2;
            var date = localDateTimeNow.ToString("h:mm tt");
            var time = localDateTimeNow.ToString("dd MMMM yyyy");

            var address = "Alajuela";
            var institution = "Escuela Manuela Santamaría Rodríguez";
            byte[] signatureBytes = Convert.FromBase64String(formData.signature);
            Image signatureImg = new Image(ImageDataFactory.Create(signatureBytes));


            using (var memoryStream = new MemoryStream())
            {
                using (var existingPdf = new PdfReader(pdfPath))
                using (var newPdf = new PdfWriter(memoryStream))
                using (var pdfDocument = new PdfDocument(existingPdf, newPdf))
                {

                    var form = PdfAcroForm.GetAcroForm(pdfDocument, true);

                    form.GetField("lugar").SetValue(formData.location).SetReadOnly(true);
                    form.GetField("hora").SetValue(date).SetReadOnly(true);
                    form.GetField("fecha").SetValue(time).SetReadOnly(true);
                    form.GetField("nombre").SetValue(fullName).SetReadOnly(true);
                    form.GetField("cedula").SetValue(formData.idCard).SetReadOnly(true);
                    form.GetField("numIncapacidad").SetValue(formData.numInability).SetReadOnly(true);
                    form.GetField("nombre2").SetValue(fullName).SetReadOnly(true);
                    form.GetField("institucion").SetValue(institution).SetReadOnly(true);
                    form.GetField("puesto").SetValue(formData.position).SetReadOnly(true);
                    form.GetField("direccion").SetValue(address).SetReadOnly(true);
                    form.GetField("rigeFecha").SetValue(formData.effectiveDate.ToString("dd'/'MM'/'yyyy")).SetReadOnly(true);
                    form.GetField("venceFecha").SetValue(formData.expiresDate.ToString("dd'/'MM'/'yyyy")).SetReadOnly(true);
                    form.GetField("emitido").SetValue(formData.issuedBy).SetReadOnly(true);
                    form.GetField("motivo").SetValue(formData.reason).SetReadOnly(true);
                    form.GetField("otro").SetValue(formData.reasonOther).SetReadOnly(true);

                    form.GetField("entregaNombre").SetValue(fullName).SetReadOnly(true);
                    form.GetField("entregaCedula").SetValue(formData.idCard).SetReadOnly(true);

                    var signatureField = form.GetField("firma");

                    Rectangle rect = signatureField.GetWidgets()[0].GetRectangle().ToRectangle();
                    signatureImg.SetFixedPosition(rect.GetLeft(), rect.GetBottom());
                    signatureImg.ScaleToFit(rect.GetWidth(), rect.GetHeight());
                    Canvas canvas = new Canvas(pdfDocument.GetPage(1), rect);
                    canvas.Add(signatureImg);

                    form.FlattenFields();
                    pdfDocument.Close();
                }

                using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
                {
                    var query = "INSERT INTO 'inability_file' (filename, filedate, filecontent, user_firstname, user_lastname1, user_lastname2) " +
                        "VALUES (@filename, @filedate, @filecontent, @name, @lastname1, @lastname2)";

                    var filecontent = memoryStream.ToArray();
                    var filedate = localDateTimeNow;

                    context.Execute(query, new { filename, filedate, filecontent, name, lastname1, lastname2 });
                }
            }
        }

        public void SubmitPermissionForm(IConfiguration _config, PermissionFormModel formData, IEmailService _emailService, string callbackURL)
        {

            var localTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");
            var localDateTimeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimezone);

            string year = localDateTimeNow.Date.Year.ToString();
            string month = localDateTimeNow.Date.ToString("MMM").TrimEnd('.').ToUpper();
            string day = localDateTimeNow.Date.ToString("dd");

            if (!String.IsNullOrWhiteSpace(formData.signature))
            {
                formData.signature = formData.signature.Split(",")[1];
            }

            string consecutive_section_1 = "0000";
            string consecutive_section_2 = "DREA";
            string consecutive_section_3 = year;
            string consecutive = $"{consecutive_section_1}-{consecutive_section_2}-{consecutive_section_3}";

            //
            string area = "SUPERVISIÓN CIRCUITO 02 - DREA";
            string workingHoursRange = "De " + formData.fromWorkingHours.ToString("hh:mm tt") + " a " + formData.toWorkingHours.ToString("hh:mm tt");

            string justifHour = formData.justificationDate.ToString("hh");
            string justifMinutes = formData.justificationDate.ToString("mm");
            string justifSeconds = formData.justificationDate.ToString("ss");
            string justifAM_PM = formData.justificationDate.ToString("tt");

            string justifYear = formData.justificationDate.Year.ToString();
            string justifMonth = formData.justificationDate.ToString("MMM").TrimEnd('.').ToUpper();
            string justifDay = formData.justificationDate.ToString("dd");

            byte[] signatureBytes = Convert.FromBase64String(formData.signature);
            Image signatureImg = new Image(ImageDataFactory.Create(signatureBytes));


            using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
            {
                var query = "SELECT consecutive FROM consecutive_ids ORDER BY id DESC LIMIT 1";

                var response = context.Query<string>(query).FirstOrDefault();

                if (response != null)
                {
                    string[] lastConsecutive = response.ToString().Split('-');
                    consecutive_section_3 = lastConsecutive[2];

                    if (year.Equals(consecutive_section_3))
                    {
                        consecutive_section_1 = (int.Parse(lastConsecutive[0]) + 1).ToString("D4");
                        consecutive = $"{consecutive_section_1}-{consecutive_section_2}-{consecutive_section_3}";
                    }
                    else
                    {
                        consecutive = $"0000-{consecutive_section_2}-{year}";
                    }
                }
                else
                {
                    consecutive = $"0000-{consecutive_section_2}-{year}";
                }
            }


            var pdfPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Resources", "permission_form_template.pdf");

            using (var memoryStream = new MemoryStream())
            {
                using (var existingPdf = new PdfReader(pdfPath))
                using (var newPdf = new PdfWriter(memoryStream))
                using (var pdfDocument = new PdfDocument(existingPdf, newPdf))
                {

                    var form = PdfAcroForm.GetAcroForm(pdfDocument, true);

                    form.GetField("fechaDia").SetValue(day).SetReadOnly(true);
                    form.GetField("fechaMes").SetValue(month).SetReadOnly(true);
                    form.GetField("fechaAnio").SetValue(year).SetReadOnly(true);

                    form.GetField("consec01").SetValue(consecutive_section_1).SetReadOnly(true);
                    form.GetField("consec02").SetValue(consecutive_section_2).SetReadOnly(true);
                    form.GetField("consec03").SetValue(year).SetReadOnly(true);

                    form.GetField("area").SetValue(area).SetReadOnly(true);
                    form.GetField("cedula").SetValue(formData.idCard).SetReadOnly(true);

                    form.GetField("nombre").SetValue(formData.name).SetReadOnly(true);
                    form.GetField("apellido1").SetValue(formData.lastname1).SetReadOnly(true);
                    form.GetField("apellido2").SetValue(formData.lastname2).SetReadOnly(true);

                    form.GetField("modalidad").SetValue(formData.workMode).SetReadOnly(true);

                    form.GetField("horario").SetValue(workingHoursRange).SetReadOnly(true);

                    form.GetField("tipo").SetValue(formData.justificationType).SetReadOnly(true);

                    if (formData.justificationType.Equals("entrada"))
                    {
                        form.GetField("horaEntrada").SetValue(justifHour).SetReadOnly(true);
                        form.GetField("minsEntrada").SetValue(justifMinutes).SetReadOnly(true);
                        form.GetField("segsEntrada").SetValue(justifSeconds).SetReadOnly(true);
                        form.GetField("ampmEntrada").SetValue(justifAM_PM).SetReadOnly(true);
                        form.GetField("diaEntrada").SetValue(justifDay).SetReadOnly(true);
                        form.GetField("mesEntrada").SetValue(justifMonth).SetReadOnly(true);
                        form.GetField("anioEntrada").SetValue(justifYear).SetReadOnly(true);
                    }
                    else
                    {
                        form.GetField("horaSalida").SetValue(justifHour).SetReadOnly(true);
                        form.GetField("minsSalida").SetValue(justifMinutes).SetReadOnly(true);
                        form.GetField("segsSalida").SetValue(justifSeconds).SetReadOnly(true);
                        form.GetField("ampmSalida").SetValue(justifAM_PM).SetReadOnly(true);
                        form.GetField("diaSalida").SetValue(justifDay).SetReadOnly(true);
                        form.GetField("mesSalida").SetValue(justifMonth).SetReadOnly(true);
                        form.GetField("anioSalida").SetValue(justifYear).SetReadOnly(true);
                    }

                    form.GetField("motivo").SetValue(formData.reason).SetReadOnly(true);

                    form.GetField("observaciones").SetValue(formData.additionalComms).SetReadOnly(true);

                    var signatureField = form.GetField("firma");

                    Rectangle rect = signatureField.GetWidgets()[0].GetRectangle().ToRectangle();
                    signatureImg.SetFixedPosition(rect.GetLeft(), rect.GetBottom());
                    signatureImg.ScaleToFit(rect.GetWidth(), rect.GetHeight());
                    Canvas canvas = new Canvas(pdfDocument.GetPage(1), rect);
                    canvas.Add(signatureImg);

                    form.FlattenFields();
                    pdfDocument.Close();
                }

                using (var context = new SQLiteConnection(_config.GetSection("ConnectionStrings:DBConnection").Value))
                {
                    var query1 = "INSERT INTO permission_file (file_consecutive_id, filename, filedate, filecontent, user_firstname, user_lastname1, user_lastname2)" +
                        "VALUES (@consecutive, @filename, @filedate, @filecontent, @name, @lastname1, @lastname2)";

                    var query2 = "INSERT INTO consecutive_ids (consecutive) VALUES (@consecutive)";

                    var filecontent = memoryStream.ToArray();
                    var filedate = localDateTimeNow;
                    var name = formData.name;
                    var lastname1 = formData.lastname1;
                    var lastname2 = formData.lastname2;
                    var filename = $"permiso_{consecutive_section_1}_{formData.name + "_" + formData.lastname1}.pdf";

                    context.Execute(query1, new { consecutive, filename, filedate, filecontent, name, lastname1, lastname2 });
                    context.Execute(query2, new { consecutive });
                }

                EmailDto emailinfo = new EmailDto();
                emailinfo.Subject = "Nueva Solicitud de Permiso Generada";
                emailinfo.To = "esc.manuelasantamaria@mep.go.cr";
                emailinfo.Body = $"<h1>Escuela Manuela Santamaría Rodríguez</h1><br><p>Le informamos que se ha generado una nueva solicitud de permiso ({consecutive}) en el sistema." +
                   $"<br><p>Ingrese a <strong><a href='{callbackURL}'>este enlace</a></strong> con un usuario administrador válido para revisarlo.</p> ";

                _emailService.SendEmail(emailinfo);
            }
        }
    }
}