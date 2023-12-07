using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Persistence.SqlServer.Migrations
{
    public partial class ChangeIdentityeSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs",
                schema: "log");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "identity",
                newName: "UserTokens",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "identity",
                newName: "Users",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "identity",
                newName: "UserRoles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "identity",
                newName: "UserLogins",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "identity",
                newName: "UserClaims",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "identity",
                newName: "Roles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "identity",
                newName: "RoleClaims",
                newSchema: "auth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "auth",
                newName: "UserTokens",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "auth",
                newName: "Users",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "auth",
                newName: "UserRoles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "auth",
                newName: "UserLogins",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "auth",
                newName: "UserClaims",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "auth",
                newName: "Roles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "auth",
                newName: "RoleClaims",
                newSchema: "identity");

            migrationBuilder.CreateTable(
                name: "Logs",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassMethod = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FullReferer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IdLevelError = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InnerException = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LevelError = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Logger = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MachineIP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RemoteAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }
    }
}
