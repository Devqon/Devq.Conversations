using Devq.Conversations.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Conversations.Handlers
{
    public class MessagePartHandler : ContentHandler
    {
        public MessagePartHandler(IRepository<MessagePartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}