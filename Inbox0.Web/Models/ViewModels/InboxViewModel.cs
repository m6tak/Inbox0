using Inbox0.Core.Models.Database;

namespace Inbox0.Web.Models.ViewModels
{
    public class InboxViewModel : BaseViewModel
    {
        public bool NoMailAccount { get; init; } = false;
        public string SelectedMailAccountId { get; set; }
        public List<InboxConversation> LoadedConversations { get; set; } = new List<InboxConversation>();
    }
}
