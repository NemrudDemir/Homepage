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

        public string ResultHtmlCode { get; private set; }

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
            bool wasSuccessful = true;
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = $"Homepage contact - {Name}",
                    Body = $"Name: {Name}{Environment.NewLine}" +
                        $"E-Mail: {EmailAddress}{Environment.NewLine}" +
                        $"Message: {Message}"
                })
                {
                    smtp.Send(message);
                }
            } catch //TODO do something with the exception
            {
                wasSuccessful = false;
            } finally
            {
                SetResultContent(wasSuccessful, settings.Value.ToMailAddress);
            }
        }

        private void SetResultContent(bool WasSuccessful, string receiverMail)
        {
            ///template for result div
            ///{0} = class: success/danger for resultText styling
            ///{1} = svg image name in 'img'-directory | success/danger
            ///{2} = resultText
            ///{3} = additional html below resultText
            const string template = "<div id='result-overlay' class='overlay'>" +
                        "<div id='sendResult'>" +
                            "<img id='sendResult-image' src='img/{1}.svg'>" +
                            "<span id='resultText' class='{0}'>{2}</span>" +
                            "{3}" +
                        "</div>" +
                    "</div>";

            if (WasSuccessful)
            {
                ResultHtmlCode = string.Format(template, "success", "success", "Mail sent!", "");
            } else
            {
                ResultHtmlCode = 
                    string.Format(template, "danger", "danger", "Sending failed!", 
                    $"<a id='mailTo' " +
                    $"href='mailto:{receiverMail}?body={Helpers.Url.Escape(Message)}'>" +
                    $"Send with mail-client" +
                    $"</a>");
            }
        }
    }
}
