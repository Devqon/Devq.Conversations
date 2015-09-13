using Devq.Conversations.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;

namespace Devq.Conversations.Drivers
{
    public class ConversationablePartDriver : ContentPartDriver<ConversationablePart> {

        private readonly IWorkContextAccessor _workContextAccessor;
        public ConversationablePartDriver(IWorkContextAccessor workContextAccessor) {
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(ConversationablePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Conversationable", () => {

                var user = _workContextAccessor.GetContext().CurrentUser;
                if (user == null)
                    return null;

                if (part.As<CommonPart>().Owner.Id == user.Id)
                    return null;

                // Only if the content item is not of the user itself
                return shapeHelper.Parts_Conversationable();
            });
        }
    }
}