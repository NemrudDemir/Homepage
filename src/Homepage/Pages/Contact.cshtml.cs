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
            var fromAddress = new MailAddress(_settings.Value.MailAddress, Name);
            var toAddress = new MailAddress(_settings.Value.ToMailAddress, Owner.FullName);

            var smtp = new SmtpClient
            {
                Host = _settings.Value.Host,
                Port = _settings.Value.Port,
                EnableSsl = _settings.Value.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _settings.Value.Password)
            };

            var wasSuccessful = true;
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

                _logger.LogInformation("Mail successfully sent!");
            } catch(Exception ex)
            {
                wasSuccessful = false;
                _logger.LogError(ex, "Error while sending mail!");
            } finally
            {
                SetResultContent(wasSuccessful, _settings.Value.ToMailAddress);
                smtp.Dispose();
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
