using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.DAL.Migrations
{
    public partial class Dialog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDialogs_Dialogs_DialogId",
                table: "UserDialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDialogs_AspNetUsers_UserId",
                table: "UserDialogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDialogs",
                table: "UserDialogs");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "UserDialogs",
                newName: "UserDialog");

            migrationBuilder.RenameIndex(
                name: "IX_UserDialogs_DialogId",
                table: "UserDialog",
                newName: "IX_UserDialog_DialogId");

            migrationBuilder.AddColumn<string>(
                name: "ContactId",
                table: "Dialogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Dialogs",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDialog",
                table: "UserDialog",
                columns: new[] { "UserId", "DialogId" });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_ContactId",
                table: "Dialogs",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_UserId",
                table: "Dialogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_ContactId",
                table: "Dialogs",
                column: "ContactId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_AspNetUsers_UserId",
                table: "Dialogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDialog_Dialogs_DialogId",
                table: "UserDialog",
                column: "DialogId",
                principalTable: "Dialogs",
                principalColumn: "DialogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDialog_AspNetUsers_UserId",
                table: "UserDialog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_ContactId",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_AspNetUsers_UserId",
                table: "Dialogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDialog_Dialogs_DialogId",
                table: "UserDialog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDialog_AspNetUsers_UserId",
                table: "UserDialog");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_ContactId",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_UserId",
                table: "Dialogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDialog",
                table: "UserDialog");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Dialogs");

            migrationBuilder.RenameTable(
                name: "UserDialog",
                newName: "UserDialogs");

            migrationBuilder.RenameIndex(
                name: "IX_UserDialog_DialogId",
                table: "UserDialogs",
                newName: "IX_UserDialogs_DialogId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDialogs",
                table: "UserDialogs",
                columns: new[] { "UserId", "DialogId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserDialogs_Dialogs_DialogId",
                table: "UserDialogs",
                column: "DialogId",
                principalTable: "Dialogs",
                principalColumn: "DialogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDialogs_AspNetUsers_UserId",
                table: "UserDialogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
