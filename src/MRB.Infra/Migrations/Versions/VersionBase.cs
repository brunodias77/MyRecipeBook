using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace MRB.Infra.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            .WithColumn("Id").AsCustom("CHAR(36)").NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
            .WithColumn("CreatedOn").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable();
    }
}