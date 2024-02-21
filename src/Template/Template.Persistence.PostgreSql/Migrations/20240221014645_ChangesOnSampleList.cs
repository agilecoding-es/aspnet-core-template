using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Persistence.PostgreSql.Migrations
{
    public partial class ChangesOnSampleList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "sample",
                table: "samplelists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "sample",
                table: "sampleitems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "sample",
                table: "samplelists");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "sample",
                table: "sampleitems");
        }
    }
}
