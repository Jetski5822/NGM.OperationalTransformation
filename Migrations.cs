using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace NGM.OperationalTransformation {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg.WithPart("ContentPadPart"));
        
            return 1;
        }

        public int UpdateFrom1() {
            ContentDefinitionManager.AlterTypeDefinition("User", cfg => cfg.RemovePart("ContentPadPart"));
            ContentDefinitionManager.AlterTypeDefinition("Page", cfg => cfg.WithPart("ContentPadPart"));

            return 2;
        }
    }
}