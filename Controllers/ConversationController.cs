using System.Linq;
using System.Web.Mvc;
using Devq.Conversations.Models;
using Devq.Conversations.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Themes;

namespace Devq.Conversations.Controllers
{
    [Themed]
    public class ConversationController : Controller, IUpdateModel {
        private readonly IConversationService _conversationService;
        private readonly IContentManager _contentManager;
        private readonly IOrchardServices _services;

        public ConversationController(IConversationService conversationService, IContentManager contentManager, IOrchardServices services, IShapeFactory shapeFactory) {
            _conversationService = conversationService;
            _contentManager = contentManager;
            _services = services;

            Shape = shapeFactory;
        }

        public dynamic Shape { get; set; }

        public ActionResult Index(int id = 0) {

            var currentUser = _services.WorkContext.CurrentUser;
            if (currentUser == null) {
                return HttpNotFound();
            }

            if (id > 0) {

                var conversation = _conversationService
                    .GetConversationQuery()
                    .Where<ConversationPartRecord>(c => c.Id == id)
                    .List()
                    .FirstOrDefault();

                if (conversation == null)
                    return HttpNotFound();

                // Not in the conversation
                if (conversation.As<CommonPart>().Owner.Id != currentUser.Id && conversation.TargetId != currentUser.Id) {
                    return HttpNotFound();
                }

                var shape = _contentManager.BuildDisplay(conversation, "Detail");

                return View(shape);
            }

            var conversations = _conversationService
                .GetConversationsByUser(currentUser.Id);

            var list = Shape.List();
            list.AddRange(conversations.Select(c => _contentManager.BuildDisplay(c, "Summary")));

            return View(list);
        }

        public ActionResult StartConversation(int subjectId) {

            var user = _services.WorkContext.CurrentUser;
            if (user == null)
                return HttpNotFound();

            var subject = _contentManager.Get(subjectId);
            if (subject == null)
                return HttpNotFound();
            var common = subject.As<CommonPart>();
            if (common == null)
                return HttpNotFound();

            // Target is owner
            var owner = common.Owner;
            // Cannot create a conversation with yourself
            //if (owner.Id == user.Id)
            //    return HttpNotFound();

            var existing = _conversationService
                .GetConversationQuery()
                .Where<ConversationPartRecord>(c => c.SubjectId == subjectId && c.InitiatorId == user.Id && c.TargetId == owner.Id)
                .List()
                .FirstOrDefault();

            // Existing conversation, redirect to it
            if (existing != null)
                return RedirectToAction("Index", new {id = existing.Id});

            var newConversation = _contentManager.New<ConversationPart>(Constants.ConversationTypeName);
            newConversation.Subject = subject;
            newConversation.InitiatorId = user.Id;
            newConversation.TargetId = owner.Id;
            newConversation.As<CommonPart>().Owner = user;

            _contentManager.Create(newConversation);

            var shape = _contentManager.BuildDisplay(newConversation, "Detail");

            return View("Index", shape);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateMessage(int conversationId) {

            var user = _services.WorkContext.CurrentUser;
            if (user == null)
                return HttpNotFound();

            // Create -> update [-> cancel]
            var message = _contentManager.New<MessagePart>(Constants.MessageTypeName);
            _contentManager.UpdateEditor(message, this);

            if (!ModelState.IsValid) {
                _services.TransactionManager.Cancel();
            }
            else {
                var conversation = _contentManager.Get<ConversationPart>(conversationId);
                message.Author = user.Id;
                message.Target = conversation.InitiatorId == user.Id ? conversation.TargetId : conversation.InitiatorId;
                message.As<CommonPart>().Container = conversation.ContentItem;

                _contentManager.Create(message);
            }

            return RedirectToAction("Index", new {id = conversationId});
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}