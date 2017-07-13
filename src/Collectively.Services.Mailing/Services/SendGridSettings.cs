namespace Collectively.Services.Mailing.Services
{
    public class SendGridSettings
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string NoReplyEmailAccount { get; set; }
        public string DefaultCulture { get; set; }
    }
}