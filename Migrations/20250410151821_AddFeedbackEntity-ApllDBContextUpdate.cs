using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackEntityApllDBContextUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientFeedbacks",
                table: "PatientFeedbacks");

            migrationBuilder.RenameTable(
                name: "PatientFeedbacks",
                newName: "PatientFeedbacks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientFeedbacks",
                table: "PatientFeedbacks",
                column: "feedbackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientFeedbacks",
                table: "PatientFeedbacks");

            migrationBuilder.RenameTable(
                name: "PatientFeedbacks",
                newName: "PatientFeedbacks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientFeedbacks",
                table: "PatientFeedbacks",
                column: "feedbackId");
        }
    }
}
