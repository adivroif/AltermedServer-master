using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class AddSlotForKeyToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_startSlot",
                table: "Appointments",
                column: "startSlot");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AppointmentSlots_startSlot",
                table: "Appointments",
                column: "startSlot",
                principalTable: "AppointmentSlots",
                principalColumn: "slotid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AppointmentSlots_startSlot",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_startSlot",
                table: "Appointments");
        }
    }
}
