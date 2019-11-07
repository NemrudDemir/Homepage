namespace Homepage.Settings
{
    public class MailSettings
    {
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string ToMailAddress { get; set; }
        public bool EnableSsl { get; set; }
    }
}
