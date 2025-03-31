using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlaceFromAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Address_placeId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_placeId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "placeId",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "placeId",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_placeId",
                table: "Appointments",
                column: "placeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Address_placeId",
                table: "Appointments",
                column: "placeId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
