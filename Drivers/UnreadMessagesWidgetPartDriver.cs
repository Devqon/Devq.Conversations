using Devq.Conversations.Models;
using Devq.Conversations.Services;
using Orchard.ContentManagement.Drivers;

namespace Devq.Conversations.Drivers
{
    public class UnreadMessagesWidgetPartDriver : ContentPartDriver<UnreadMessagesWidgetPart> {

        private readonly IConversationService _conversationService;
        public UnreadMessagesWidgetPartDriver(IConversationService conversationService) {
            _conversationService = conversationService;
        }

        protected override DriverResult Display(UnreadMessagesWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_UnreadMessagesWidget", () => {
                var unreadMessagesCount = _conversationService.GetUnreadMessagesCount(part.Id);

                return shapeHelper.Parts_User_UnreadMessages(Count: unreadMessagesCount);
            });
        }
    }
}