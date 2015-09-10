using System;
using System.Linq;
using Devq.Conversations.Models;
using Devq.Conversations.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Settings;
using Orchard.UI.Navigation;

namespace Devq.Conversations.Drivers
{
    public class ConversationPartDriver : ContentPartDriver<ConversationPart> {
        private readonly IConversationService _conversationService;
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ISiteService _siteService;

        public ConversationPartDriver(IConversationService conversationService, IContentManager contentManager, IWorkContextAccessor workContextAccessor, ISiteService siteService) {
            _conversationService = conversationService;
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
            _siteService = siteService;
        }

        protected override DriverResult Display(ConversationPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_Conversation_Subject", () => shapeHelper.Parts_Conversation_Subject(Subject: part.Subject)),
                ContentShape("Parts_Conversation_UnreadMessagesCount", () => {
                    var user = _workContextAccessor.GetContext().CurrentUser;
                    if (user == null)
                        return null;

                    var unreadMessagesCount = _conversationService.GetUnreadMessagesCount(user.Id, part.Id);
                    return shapeHelper.Parts_Conversation_UnreadMessagesCount(Count: unreadMessagesCount);
                }),
                ContentShape("Parts_Conversation_Messages", () => {

                    var pagerParameters = new PagerParameters();
                    var httpContext = _workContextAccessor.GetContext().HttpContext;
                    if (httpContext != null)
                    {
                        pagerParameters.Page = Convert.ToInt32(httpContext.Request.QueryString["page"]);
                    }

                    var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

                    var list = shapeHelper.List();
                    var messages = _conversationService.GetMessages(part.Id);
                    var totalCount = messages.Count();

                    var pagerShape = pager.PageSize == 0 ? null : shapeHelper.Pager(pager).TotalItemCount(totalCount);
                    var pagedMessages = messages
                        .OrderByDescending<CommonPartRecord>(c => c.CreatedUtc)
                        .Slice(pager.GetStartIndex(), pager.PageSize);

                    list.AddRange(pagedMessages.Select(m => _contentManager.BuildDisplay(m)));

                    return shapeHelper.Parts_Conversation_Messages(List: list, Pager: pagerShape);
                }),
                ContentShape("Parts_Message_Form", () => {
                    var message = _contentManager.New(Constants.MessageTypeName);
                    return shapeHelper.Parts_Message_Form(MessageEditorShape: _contentManager.BuildEditor(message), ConversationId: part.Id);
                }));
        }
    }
}