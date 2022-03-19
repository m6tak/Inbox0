namespace Inbox0.Web.Models
{
    public record SelectInboxRequest(string mailId, string inboxId);
    public record OpenConversationRequest(string conversationId);
}
