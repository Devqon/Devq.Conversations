using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Devq.Conversations.Models
{
    public class MessagePart : ContentPart<MessagePartRecord>
    {
        public bool Read {
            get { return Retrieve(r => r.Read); }
            set { Store(r => r.Read, value); }
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

        public virtual bool Read { get; set; }
        public virtual int Target { get; set; }
        public virtual int Author { get; set; }
    }
}