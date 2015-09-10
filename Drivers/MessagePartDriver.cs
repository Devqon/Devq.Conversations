using Devq.Conversations.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Devq.Conversations.Drivers
{
    public class MessagePartDriver : ContentPartDriver<MessagePart> {
        protected override DriverResult Display(MessagePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Message", () => shapeHelper.Parts_Message(Part: part));
        }

        protected override DriverResult Editor(MessagePart part, dynamic shapeHelper) {
            return ContentShape("Parts_Message_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "Parts/Message",
                Prefix: Prefix,
                Part: part));
        }

        protected override DriverResult Editor(MessagePart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}