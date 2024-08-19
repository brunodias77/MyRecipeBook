using FluentMigrator;

namespace MRB.Infra.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes and Ingredients information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        Create.Table("Recipes")
            .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");

        Create.Table("Ingredients")
            .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Item").AsString(255).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

        Create.Table("Instructions")
            .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("FK_Instruction_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

        Create.Table("DishTypes")
            .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("FK_DishType_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);
    }
}