using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Records;
using Orchard.ContentManagement.Utilities;
using Orchard.Core.Title.Models;

namespace Devq.Conversations.Models
{
    public class ConversationPart : ContentPart<ConversationPartRecord>, ITitleAspect
    {
        internal LazyField<IContent> _subjectField = new LazyField<IContent>(); 

        public IContent Subject {
            get { return _subjectField.Value; }
            set { _subjectField.Value = value; }
        }

        public string Title {
            get { return Subject.As<TitlePart>().Title; }
        }

        public int InitiatorId
        {
            get { return Retrieve(r => r.InitiatorId); }
            set { Store(r => r.InitiatorId, value); }
        }

        public int TargetId
        {
            get { return Retrieve(r => r.TargetId); }
            set { Store(r => r.TargetId, value); }
        }
    }

    public class ConversationPartRecord : ContentPartRecord {
        public virtual int InitiatorId { get; set; }
        public virtual int TargetId { get; set; }
        public virtual int SubjectId { get; set; }
    }
}