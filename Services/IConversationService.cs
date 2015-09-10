using System.Collections.Generic;
using Devq.Conversations.Models;
using Orchard;
using Orchard.ContentManagement;

namespace Devq.Conversations.Services {
    public interface IConversationService : IDependency {
        IContentQuery<ConversationPart> GetConversationQuery();
        ConversationPart GetConversationBySubject(int subjectId);
        IEnumerable<ConversationPart> GetConversationsByUser(int userId);
        IContentQuery<MessagePart> GetMessages(int conversationId);
        IContentQuery<MessagePart> GetMessagesQuery();
        IContentQuery<MessagePart> GetConversationMessagesQuery(int conversationId);
        IContentQuery<MessagePart> GetUnreadMessagesQuery(int userId, int conversationId = 0);
        int GetUnreadMessagesCount(int userId, int conversationId = 0);
    }
}