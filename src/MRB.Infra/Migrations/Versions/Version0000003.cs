using FluentMigrator;

namespace MRB.Infra.Migrations.Versions;

[Migration(DatabaseVersions.IMAGE_FOR_RECIPES, "Add collumn on recipe table to save images")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        Alter.Table("Recipes").AddColumn("ImageIdentifier").AsString().Nullable();
    }
}