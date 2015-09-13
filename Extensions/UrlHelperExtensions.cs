using System.Web.Mvc;
using Orchard.ContentManagement;

namespace Devq.Conversations.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Conversations(this UrlHelper urlHelper) {
            return urlHelper.Action("Index", "Conversation", new {area = "Devq.Conversations"});
        }

        public static string Conversation(this UrlHelper urlHelper, int conversationId) {
            return urlHelper.Action("Details", "Conversation", new { area = "Devq.Conversations", id = conversationId });
        }

        public static string StartConversation(this UrlHelper urlHelper, IContent content) {
            return urlHelper.Action("StartConversation", "Conversation", new {area = "Devq.Conversations", subjectId = content.Id});
        }
    }
}