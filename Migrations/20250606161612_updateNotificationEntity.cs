using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class updateNotificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
      name: "SavedNotifications",
      newName: "StoredNotifications"
  );
            migrationBuilder.CreateIndex(
                name: "IX_StoredNotifications_userId_isRead",
                table: "StoredNotifications",
                columns: new[] { "userId", "isRead" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
        name: "StoredNotifications",
        newName: "SavedNotifications"
    );
            migrationBuilder.DropIndex(
                name: "IX_StoredNotifications_userId_isRead",
                table: "StoredNotifications");
        }
    }
}
