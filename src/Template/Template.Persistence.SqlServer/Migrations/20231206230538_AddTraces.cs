using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Persistence.SqlServer.Migrations
{
    public partial class AddTraces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.CreateTable(
                name: "Exceptions",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdLevelError = table.Column<int>(type: "int", nullable: false),
                    LevelError = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InnerException = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClassMethod = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MachineIP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Logger = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RemoteAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullReferer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traces",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdLevelError = table.Column<int>(type: "int", nullable: false),
                    LevelError = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InnerException = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClassMethod = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MachineIP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Logger = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RemoteAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullReferer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traces", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exceptions",
                schema: "log");

            migrationBuilder.DropTable(
                name: "Traces",
                schema: "log");
        }
    }
}
