using System.Collections.Generic;
using System.Linq;
using Devq.Conversations.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;

namespace Devq.Conversations.Services
{
    public class ConversationService : IConversationService {
        private readonly IContentManager _contentManager;
        public ConversationService(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public IContentQuery<ConversationPart> GetConversationQuery() {
            return _contentManager
                .Query<ConversationPart, ConversationPartRecord>(Constants.ConversationTypeName);
        }

        public ConversationPart GetConversationBySubject(int subjectId) {
            return GetConversationQuery()
                .Where<ConversationPartRecord>(c => c.SubjectId == subjectId)
                .List()
                .FirstOrDefault();
        }

        public IEnumerable<ConversationPart> GetConversationsByUser(int userId) {
            return GetConversationQuery()
                .Where<CommonPartRecord>(c => c.OwnerId == userId)
                .List();
        }

        public IContentQuery<MessagePart> GetMessages(int conversationId) {
            var conversation = _contentManager.Get<ConversationPart>(conversationId);
            if (conversation == null)
                return null;

            var messages = GetConversationMessagesQuery(conversationId);

            return messages;
        }

        public IContentQuery<MessagePart> GetMessagesQuery() {
            return _contentManager
                .Query<MessagePart>(Constants.MessageTypeName);
        }

        public IContentQuery<MessagePart> GetConversationMessagesQuery(int conversationId) {
            return GetMessagesQuery()
                .Where<CommonPartRecord>(c => c.Container.Id == conversationId);
        }

        public IContentQuery<MessagePart> GetUnreadMessagesQuery(int userId, int conversationId = 0) {
            if (conversationId > 0) {
                return GetConversationMessagesQuery(conversationId)
                    .Where<CommonPartRecord>(c => c.Container.Id == conversationId);
            }

            return GetMessagesQuery()
                .Where<CommonPartRecord>(c => c.Container.Id == conversationId);
        } 

        public int GetUnreadMessagesCount(int userId, int conversationId = 0) {
            return GetUnreadMessagesQuery(userId, conversationId).Count();
        }
    }
}