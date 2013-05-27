using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace NGM.OperationalTransformation {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            ContentDefinitionManager.AlterPartDefinition("ContentPadPart", builder => builder
                .Attachable()
            );

            ContentDefinitionManager.AlterTypeDefinition("Page", cfg => cfg.WithPart("ContentPadPart"));
        
            return 3;
        }

        public int UpdateFrom1() {
            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg.RemovePart("ContentPadPart"));
            ContentDefinitionManager.AlterTypeDefinition("Page", cfg => cfg.WithPart("ContentPadPart"));

            return 2;
        }

        public int UpdateFrom2() {
            ContentDefinitionManager.AlterPartDefinition("ContentPadPart", builder => builder
                .Attachable()
            );

            return 3;
        }
    }
}