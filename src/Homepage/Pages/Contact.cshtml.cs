using System;
using System.Net;
using System.Net.Mail;
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

        private IOptions<MailSettings> _settings;
        private ILogger<Contact> _logger;
        public Contact(IOptions<MailSettings> settings, ILogger<Contact> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public void OnGet()
        {
            //Nothing to do
        }

        public void OnPost()
        {
            var wasSuccessful = true;
            try
            {
                var fromAddress = new MailAddress(_settings.Value.MailAddress, Name);
                var toAddress = new MailAddress(_settings.Value.ToMailAddress, Owner.FullName);
                using(MailMessage mail = new MailMessage()) {
                    mail.From = fromAddress;
                    mail.To.Add(toAddress);
                    mail.Subject = $"Homepage contact - {Name}";
                    mail.Body = $"Name: {Name}{Environment.NewLine}" +
                        $"E-Mail: {EmailAddress}{Environment.NewLine}" +
                        $"Message: {Message}";

                    using(var smtp = new SmtpClient(_settings.Value.Host, _settings.Value.Port)) {
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(fromAddress.Address, _settings.Value.Password);
                        smtp.Send(mail);
                    }
                }

                _logger.LogInformation("Mail successfully sent!");
            } catch(Exception ex)
            {
                wasSuccessful = false;
                _logger.LogError(ex, "Error while sending mail!");
            } finally
            {
                SetResultContent(wasSuccessful, _settings.Value.ToMailAddress);
            }
        }

        private void SetResultContent(bool wasSuccessful, string receiverMail)
        {
            //template for result div
            //{0} = class: success/danger for resultText styling
            //{1} = svg image name in 'img'-directory | success/danger
            //{2} = resultText
            //{3} = additional html below resultText
            const string template = "<div id='result-overlay' class='overlay'>" +
                        "<div id='sendResult'>" +
                            "<img id='sendResult-image' src='img/{1}.svg'>" +
                            "<span id='resultText' class='{0}'>{2}</span>" +
                            "{3}" +
                        "</div>" +
                    "</div>";

            if (wasSuccessful)
            {
                ResultHtmlCode = string.Format(template, "success", "success", "Mail sent!", "");
            } else
            {
                ResultHtmlCode = 
                    string.Format(template, "danger", "danger", "Sending failed!", 
                    "<a id='mailTo' " +
                    $"href='mailto:{receiverMail}?body={Helpers.Url.Escape(Message)}'>" +
                    "Send with mail-client" +
                    "</a>");
            }
        }
    }
}
