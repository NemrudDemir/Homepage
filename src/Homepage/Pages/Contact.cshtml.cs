using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Homepage.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homepage.Pages
{
    public class Contact : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string EmailAddress { get; set; }

        [BindProperty]
        public string Message { get; set; }

        IOptions<MailSettings> Settings { get; set; }

        public void OnGet()
        {
        }

        public void OnPost([FromServices]IOptions<MailSettings> settings)
        {
            var fromAddress = new MailAddress(settings.Value.MailAddress, Name);
            var toAddress = new MailAddress(settings.Value.ToMailAddress, "Nemrud Demir");

            var smtp = new SmtpClient
            {
                Host = settings.Value.Host,
                Port = settings.Value.Port,
                EnableSsl = settings.Value.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, settings.Value.Password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = $"Homepage contact - {Name}",
                Body = Message
            })
            {
                smtp.Send(message);
                int i = 5;
            }
        }
    }
}
