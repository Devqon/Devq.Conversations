using Devq.Conversations.Models;
using Devq.Conversations.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Data;

namespace Devq.Conversations.Handlers
{
    public class ConversationPartHandler : ContentHandler {
        private readonly IConversationService _conversationService;
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public ConversationPartHandler(IRepository<ConversationPartRecord> repository, IContentManager contentManager, IWorkContextAccessor workContextAccessor, IConversationService conversationService) {
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
            _conversationService = conversationService;
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ConversationPart>(SetupConversationPart);
            OnGetDisplayShape<ConversationPart>(SetMessagesRead);
        }

        private void SetMessagesRead(BuildDisplayContext ctx, ConversationPart part) {

            var user = _workContextAccessor.GetContext().CurrentUser;
            if (user == null)
                return;

            // Get all messages of the conversation where the target is current user
            var messages = _conversationService
                .GetUnreadMessagesQuery(user.Id, part.Id);    

            // Set all to read
            foreach (var message in messages.List()) {
                message.IsRead = true;
            }
        }

        private void SetupConversationPart(ActivatedContentContext ctx, ConversationPart part) {
            
            // Setup getter
            part._subjectField.Loader(subject => _contentManager.Get(part.Record.SubjectId));

            // Setup setter
            part._subjectField.Setter(subject => {
                part.Record.SubjectId = subject == null ? 0 : subject.Id;
                return subject;
            });
        }
    }
}