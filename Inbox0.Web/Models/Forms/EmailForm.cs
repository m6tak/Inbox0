namespace Inbox0.Web.Models.Forms
{
    public class EmailForm
    {
        public string Name { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string IncomingServer { get; set; } = "";
        public string IncomingProtocol { get; set; } = "";
        public int IncomingPort { get; set; } = 0;
        public string IncomingPassword { get; set; } = "";
        public string OutgoingServer { get; set; } = "";
        public string OutgoingProtocol { get; set; } = "";
        public int OutgoingPort { get; set; } = 0;
        public string OutgoingPassword { get; set; } = "";
    }
}
