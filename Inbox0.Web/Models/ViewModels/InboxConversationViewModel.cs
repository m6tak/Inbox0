namespace Inbox0.Web.Models.ViewModels
{
    public class InboxConversationViewModel : BaseViewModel
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }

    public record MessageViewModel(string Subject, string Content, string From, string To);
}
