using Devq.Conversations.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Devq.Conversations
{
    public class Migrations : DataMigrationImpl
    {
        public int Create() {

            SchemaBuilder.CreateTable(typeof (ConversationPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    
                    .Column<int>("SubjectId")
                    .Column<int>("TargetId")
                    .Column<int>("InitiatorId"));

            SchemaBuilder.CreateTable(typeof (MessagePartRecord).Name,
                table => table
                    .ContentPartRecord()

                    .Column<bool>("Read")
                    .Column<int>("Target")
                    .Column<int>("Author"));

            ContentDefinitionManager.AlterPartDefinition(typeof (MessagePart).Name,
                part => part.WithField("Message", field => field
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "small")));

            ContentDefinitionManager.AlterTypeDefinition(Constants.MessageTypeName,
                type => type
                    .Listable(false)
                    .Creatable(false)
                    
                    .WithPart(typeof (MessagePart).Name)
                    .WithPart("CommonPart"));

            ContentDefinitionManager.AlterTypeDefinition(Constants.ConversationTypeName,
                type => type
                    .Listable()
                    .Creatable(false)

                    .WithPart(typeof (ConversationPart).Name)

                    .WithPart("CommonPart"));

            return 1;
        }

        public int UpdateFrom1() {

            ContentDefinitionManager.AlterTypeDefinition("UnreadMessagesWidget",
                type => type
                    .WithPart(typeof (UnreadMessagesWidgetPart).Name)
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget"));

            ContentDefinitionManager.AlterPartDefinition(typeof (ConversationablePart).Name,
                part => part
                    .Attachable());

            return 2;
        }

        public int UpdateFrom2() {

            SchemaBuilder.AlterTable(typeof (MessagePartRecord).Name,
                table => table
                    .DropColumn("Read"));

            SchemaBuilder.AlterTable(typeof (MessagePartRecord).Name,
                table => table
                    .AddColumn<bool>("IsRead"));

            return 3;
        }
    }
}