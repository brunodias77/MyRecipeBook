using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace MRB.Infra.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Create table to save the refresh token")]
    public class Version0000004 : VersionBase
    {
        public override void Up()
        {
            CreateTable("RefreshTokens")
                        .WithColumn("Value").AsString().NotNullable()
                        .WithColumn("UserId").AsCustom("CHAR(36)").NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id");
        }
    }
}