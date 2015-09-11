using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Devq.Conversations.Models
{
    public class MessagePart : ContentPart<MessagePartRecord>
    {
        public bool IsRead {
            get { return Retrieve(r => r.IsRead); }
            set { Store(r => r.IsRead, value); }
        }

        public int Target
        {
            get { return Retrieve(r => r.Target); }
            set { Store(r => r.Target, value); }
        }

        public int Author
        {
            get { return Retrieve(r => r.Author); }
            set { Store(r => r.Author, value); }
        }
    }

    public class MessagePartRecord : ContentPartRecord {

        public virtual bool IsRead { get; set; }
        public virtual int Target { get; set; }
        public virtual int Author { get; set; }
    }
}