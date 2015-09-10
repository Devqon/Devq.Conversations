using Devq.Conversations.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Conversations.Handlers
{
    public class ConversationPartHandler : ContentHandler {
        private readonly IContentManager _contentManager;

        public ConversationPartHandler(IRepository<ConversationPartRecord> repository, IContentManager contentManager) {
            _contentManager = contentManager;
            Filters.Add(StorageFilter.For(repository));

            OnActivated<ConversationPart>(SetupConversationPart);
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