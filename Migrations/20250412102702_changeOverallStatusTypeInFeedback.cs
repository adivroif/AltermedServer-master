using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class changeOverallStatusTypeInFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<float>(
                name: "overallStatus",
                table: "PatientFeedbacks",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<int>(
                name: "overallStatus",
                table: "PatientFeedbacks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
