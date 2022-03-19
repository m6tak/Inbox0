using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inbox0.Web.Migrations
{
    public partial class MailClientDbStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_MailAccounts_OwnerId",
                table: "Inboxes");

            migrationBuilder.DropForeignKey(
                name: "FK_InboxMessages_Inboxes_InboxId",
                table: "InboxMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_MailAccounts_Users_OwnerId",
                table: "MailAccounts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "InboxMessages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "InboxMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "InboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "InboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReplyToExternalId",
                table: "InboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "InboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InboxConversations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InboxId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastMessageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageCount = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboxConversations_Inboxes_InboxId",
                        column: x => x.InboxId,
                        principalTable: "Inboxes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InboxMessages_ConversationId",
                table: "InboxMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_InboxConversations_InboxId",
                table: "InboxConversations",
                column: "InboxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_MailAccounts_OwnerId",
                table: "Inboxes",
                column: "OwnerId",
                principalTable: "MailAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InboxMessages_InboxConversations_ConversationId",
                table: "InboxMessages",
                column: "ConversationId",
                principalTable: "InboxConversations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InboxMessages_Inboxes_InboxId",
                table: "InboxMessages",
                column: "InboxId",
                principalTable: "Inboxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MailAccounts_Users_OwnerId",
                table: "MailAccounts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_MailAccounts_OwnerId",
                table: "Inboxes");

            migrationBuilder.DropForeignKey(
                name: "FK_InboxMessages_InboxConversations_ConversationId",
                table: "InboxMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_InboxMessages_Inboxes_InboxId",
                table: "InboxMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_MailAccounts_Users_OwnerId",
                table: "MailAccounts");

            migrationBuilder.DropTable(
                name: "InboxConversations");

            migrationBuilder.DropIndex(
                name: "IX_InboxMessages_ConversationId",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "From",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "ReplyToExternalId",
                table: "InboxMessages");

            migrationBuilder.DropColumn(
                name: "To",
                table: "InboxMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "InboxMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_MailAccounts_OwnerId",
                table: "Inboxes",
                column: "OwnerId",
                principalTable: "MailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboxMessages_Inboxes_InboxId",
                table: "InboxMessages",
                column: "InboxId",
                principalTable: "Inboxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MailAccounts_Users_OwnerId",
                table: "MailAccounts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
