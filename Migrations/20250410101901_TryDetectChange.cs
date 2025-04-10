using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class TryDetectChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdvanced",
                table: "Treatments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "treatmentGroup",
                table: "Treatments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdvanced",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "treatmentGroup",
                table: "Treatments");
        }
    }
}
