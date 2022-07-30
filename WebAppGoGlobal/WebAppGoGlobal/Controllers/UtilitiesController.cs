using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using WebAppGoGlobal.Data;
using WebAppGoGlobal.Models;

namespace WebAppGoGlobal.Controllers
{
    [Authorize]
    public class UtilitiesController : Controller
    {
        private readonly BookmarkContext _context;

        public UtilitiesController(BookmarkContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(string email)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(email);
                    message.From = new MailAddress("malioki008@gmail.com");
                    message.Subject = "subject";
                    message.Body = "<h1>Hello emails!</h1>";
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("malioki008@gmail.com", "sndyzgjnwmadnnyx");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(message);
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException);
            }
            return StatusCode(200, true);
        }

        [HttpGet]
        public async Task<IActionResult> ExportCsv()
        {
            var repositories = _context.Repositories.ToArray();

            var stream = new MemoryStream();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };
            using (var writer = new StreamWriter(stream, leaveOpen: true))
            {
                var csv = new CsvWriter(writer, config);
                csv.WriteRecords(repositories);
            }

            stream.Position = 0;

            return File(stream, "text/csv", fileDownloadName: "bookmarks.csv");
        }
    }
}
