using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inbox0.Web.Migrations
{
    public partial class MailAccountLastUidCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastUid",
                table: "MailAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUid",
                table: "MailAccounts");
        }
    }
}
