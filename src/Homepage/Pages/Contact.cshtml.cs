using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BitArmory.ReCaptcha;
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

        public bool RecaptchaEnabled => _recaptchaSettings?.Value?.Enabled ?? false;
        public string RecaptchaSiteKey => _recaptchaSettings?.Value?.SiteKey;

        private readonly IOptions<MailSettings> _settings;
        private readonly IOptions<RecaptchaSettings> _recaptchaSettings;
        private readonly ILogger<Contact> _logger;
        public Contact(IOptions<MailSettings> settings, IOptions<RecaptchaSettings> recaptchaSettings, ILogger<Contact> logger)
        {
            _settings = settings;
            _logger = logger;
            _recaptchaSettings = recaptchaSettings;
        }

        public void OnGet()
        {
            //Nothing to do
        }

        public async Task OnPostAsync()
        {
            if (RecaptchaEnabled)
            {
                var captchaResponse = string.Empty;
                if (Request.Form.TryGetValue(Constants.ClientResponseKey, out var formField))
                    captchaResponse = formField;

                var isCaptchaValid = await Verify(captchaResponse, HttpContext.Connection.RemoteIpAddress);
                if (!isCaptchaValid)
                {
                    SetResultContent(false, _settings.Value.ToMailAddress, "The reCAPTCHA is not valid.");
                    return;
                }
            }

            var additionalInfo = string.Empty;
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
                additionalInfo = "Internal error.";
                _logger.LogError(ex, "Error while sending mail!");
            } finally
            {
                SetResultContent(wasSuccessful, _settings.Value.ToMailAddress, additionalInfo);
            }
        }

        private async Task<bool> Verify(string captchaResponse, IPAddress clientIp)
        {
            if (string.IsNullOrEmpty(captchaResponse) || clientIp is null)
                return false;
            var captchaApi = new ReCaptchaService(); 
            return await captchaApi.Verify2Async(captchaResponse, clientIp.ToString(), _recaptchaSettings.Value.SecretKey);
        }

        private void SetResultContent(bool wasSuccessful, string receiverMail, string additionalInfo)
        {
            //template for result div
            //{0} = class: success/danger for resultText styling
            //{1} = svg image name in 'img'-directory | success/danger
            //{2} = resultText
            //{3} = additional result information
            //{4} = additional html below resultText
            const string template = "<div id='result-overlay' class='overlay'>" +
                        "<span class='close' onclick=\"$('#result-overlay').hide();\">×</span>" +
                        "<div id='sendResult'>" +
                            "<img id='sendResult-image' src='img/{1}.svg'>" +
                            "<span id='resultText' class='{0}'>{2}</span>" +
                            "<span id='resultInfo' class='{0}'>{3}</span>" +
                        "{4}" +
                        "</div>" +
                    "</div>";

            if (wasSuccessful)
            {
                ResultHtmlCode = string.Format(template, "success", "success", "Mail sent!", additionalInfo, "");
            } else
            {
                ResultHtmlCode = 
                    string.Format(template, "danger", "danger", "Sending failed!", additionalInfo,
                    "<a id='mailTo' " +
                    $"href='mailto:{receiverMail}?body={Helpers.Url.Escape(Message)}'>" +
                    "Send with mail-client" +
                    "</a>");
            }
        }
    }
}
