using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAndTreatmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Address_placeId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patients_patientId",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_placeId",
                table: "Appointments",
                newName: "IX_Appointments_placeId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_patientId",
                table: "Appointments",
                newName: "IX_Appointments_patientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "appointmentId");

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    doctorLicense = table.Column<string>(type: "text", nullable: false),
                    doctorName = table.Column<string>(type: "text", nullable: false),
                    specList = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    treatmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    treatmetName = table.Column<string>(type: "text", nullable: false),
                    treatmentDescription = table.Column<string>(type: "text", nullable: false),
                    treatmentDuration = table.Column<int>(type: "integer", nullable: false),
                    treatmentPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    treatmentPlaceId = table.Column<int>(type: "integer", nullable: false),
                    SuitCategories = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.treatmentId);
                    table.ForeignKey(
                        name: "FK_Treatments_Address_treatmentPlaceId",
                        column: x => x.treatmentPlaceId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_treatmentPlaceId",
                table: "Treatments",
                column: "treatmentPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Address_placeId",
                table: "Appointments",
                column: "placeId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_patientId",
                table: "Appointments",
                column: "patientId",
                principalTable: "Patients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Address_placeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_patientId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_placeId",
                table: "Appointment",
                newName: "IX_Appointment_placeId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_patientId",
                table: "Appointment",
                newName: "IX_Appointment_patientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "appointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Address_placeId",
                table: "Appointment",
                column: "placeId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patients_patientId",
                table: "Appointment",
                column: "patientId",
                principalTable: "Patients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
