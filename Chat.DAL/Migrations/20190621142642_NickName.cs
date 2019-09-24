using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.DAL.Migrations
{
    public partial class NickName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Contacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Contacts");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Contacts",
                nullable: false,
                defaultValue: false);
        }
    }
}
