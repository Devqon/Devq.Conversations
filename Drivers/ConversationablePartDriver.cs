using Devq.Conversations.Models;
using Orchard.ContentManagement.Drivers;

namespace Devq.Conversations.Drivers
{
    public class ConversationablePartDriver : ContentPartDriver<ConversationablePart> {
        protected override DriverResult Display(ConversationablePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Conversationable", () => shapeHelper.Parts_Conversationable());
        }
    }
}