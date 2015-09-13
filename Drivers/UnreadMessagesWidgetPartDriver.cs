using Devq.Conversations.Models;
using Devq.Conversations.Services;
using Orchard;
using Orchard.ContentManagement.Drivers;

namespace Devq.Conversations.Drivers
{
    public class UnreadMessagesWidgetPartDriver : ContentPartDriver<UnreadMessagesWidgetPart> {

        private readonly IConversationService _conversationService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public UnreadMessagesWidgetPartDriver(IConversationService conversationService, IWorkContextAccessor workContextAccessor) {
            _conversationService = conversationService;
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(UnreadMessagesWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_UnreadMessagesWidget", () => {
                var user = _workContextAccessor.GetContext().CurrentUser;
                if (user == null)
                    return null;

                var unreadMessagesCount = _conversationService.GetUnreadMessagesCount(user.Id);

                return shapeHelper.Parts_User_UnreadMessages(Count: unreadMessagesCount);
            });
        }
    }
}