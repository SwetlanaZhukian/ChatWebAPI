using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.DAL.Migrations
{
    public partial class AttachmentDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentType",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Files");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttachmentType",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Files",
                nullable: true);
        }
    }
}
