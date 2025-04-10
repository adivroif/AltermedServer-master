using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class updateEnumsTobeSerializable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Address_treatmentPlaceId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_treatmentPlaceId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "treatmentDuration",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "treatmentPlaceId",
                table: "Treatments");

            migrationBuilder.RenameColumn(
                name: "SuitCategories",
                table: "Treatments",
                newName: "suitCategories");

            migrationBuilder.RenameColumn(
                name: "treatmetName",
                table: "Treatments",
                newName: "treatmentName");

            migrationBuilder.RenameColumn(
                name: "treatmentId",
                table: "PatientRequest",
                newName: "appointmentId");

            migrationBuilder.AddColumn<string>(
                name: "date_of_treatment",
                table: "Treatments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_of_treatment",
                table: "Treatments");

            migrationBuilder.RenameColumn(
                name: "suitCategories",
                table: "Treatments",
                newName: "SuitCategories");

            migrationBuilder.RenameColumn(
                name: "treatmentName",
                table: "Treatments",
                newName: "treatmetName");

            migrationBuilder.RenameColumn(
                name: "appointmentId",
                table: "PatientRequest",
                newName: "treatmentId");

            migrationBuilder.AddColumn<int>(
                name: "treatmentDuration",
                table: "Treatments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "treatmentPlaceId",
                table: "Treatments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_treatmentPlaceId",
                table: "Treatments",
                column: "treatmentPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Address_treatmentPlaceId",
                table: "Treatments",
                column: "treatmentPlaceId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
