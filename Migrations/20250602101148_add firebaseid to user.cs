using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class addfirebaseidtouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "firebaseId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firebaseId",
                table: "Users");

        }
    }
}
