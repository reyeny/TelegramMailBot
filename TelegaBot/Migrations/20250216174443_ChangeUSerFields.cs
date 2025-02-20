using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegaBot.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUSerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramUserId",
                table: "Users",
                newName: "TelegramChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramChatId",
                table: "Users",
                newName: "TelegramUserId");
        }
    }
}
