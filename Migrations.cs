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
                    .Column<int>("Target_Id"));

            SchemaBuilder.CreateTable(typeof (MessagePartRecord).Name,
                table => table
                    .ContentPartRecord()

                    .Column<bool>("Read"));

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

            return 2;
        }

        public int UpdateFrom2() {

            SchemaBuilder.AlterTable(typeof (MessagePartRecord).Name,
                table => table.DropColumn("Author_Id"));

            SchemaBuilder.AlterTable(typeof (MessagePartRecord).Name,
                table => table.DropColumn("ConversationPartRecord_Id"));

            SchemaBuilder.AlterTable(typeof (ConversationPartRecord).Name,
                table => table
                    .DropColumn("Initiator_Id"));

            SchemaBuilder.AlterTable(typeof (ConversationPartRecord).Name,
                table => table
                    .AddColumn<int>("InitiatorId"));

            SchemaBuilder.AlterTable(typeof (ConversationPartRecord).Name,
                table => table
                    .DropColumn("Target_Id"));

            SchemaBuilder.AlterTable(typeof (ConversationPartRecord).Name,
                table => table
                    .AddColumn<int>("TargetId"));

            ContentDefinitionManager.AlterPartDefinition(typeof (MessagePart).Name,
                part => part.WithField("Message", field => field
                    .OfType("TextField")
                    .WithSetting("TextFieldSettings.Flavor", "small")));

            ContentDefinitionManager.AlterTypeDefinition(Constants.MessageTypeName, 
                type => type
                    .RemovePart("BodyPart"));

            return 3;
        }

        public int UpdateFrom3() {
            
            ContentDefinitionManager.AlterPartDefinition(typeof(ConversationablePart).Name,
                part => part
                    .Attachable());

            return 4;
        }

        public int UpdateFrom4() {

            SchemaBuilder.AlterTable(typeof (MessagePartRecord).Name,
                table => table
                    .AddColumn<int>("Target"));

            SchemaBuilder.AlterTable(typeof(MessagePartRecord).Name,
                table => table
                    .AddColumn<int>("Author"));

            return 5;
        }
    }
}