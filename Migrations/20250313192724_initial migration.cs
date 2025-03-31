using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    street = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    postalCode = table.Column<string>(type: "text", nullable: false),
                    houseNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    patientID = table.Column<string>(type: "text", nullable: false),
                    patientName = table.Column<string>(type: "text", nullable: false),
                    patientSurname = table.Column<string>(type: "text", nullable: false),
                    patientEmail = table.Column<string>(type: "text", nullable: false),
                    patientPhone = table.Column<string>(type: "text", nullable: false),
                    patientAddressId = table.Column<int>(type: "integer", nullable: false),
                    healthProvider = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<char>(type: "character(1)", nullable: false),
                    dateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.id);
                    table.ForeignKey(
                        name: "FK_Patients_Address_patientAddressId",
                        column: x => x.patientAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    startSlot = table.Column<int>(type: "integer", nullable: false),
                    treatmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    patientId = table.Column<Guid>(type: "uuid", nullable: false),
                    doctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    recordId = table.Column<Guid>(type: "uuid", nullable: false),
                    placeId = table.Column<int>(type: "integer", nullable: false),
                    statusOfAppointment = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.appointmentId);
                    table.ForeignKey(
                        name: "FK_Appointment_Address_placeId",
                        column: x => x.placeId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointment_Patients_patientId",
                        column: x => x.patientId,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecord",
                columns: table => new
                {
                    recordID = table.Column<Guid>(type: "uuid", nullable: false),
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    patientId = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    recordType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecord", x => x.recordID);
                    table.ForeignKey(
                        name: "FK_MedicalRecord_Patients_patientId",
                        column: x => x.patientId,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientRequest",
                columns: table => new
                {
                    requestId = table.Column<Guid>(type: "uuid", nullable: false),
                    patientId = table.Column<Guid>(type: "uuid", nullable: false),
                    doctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    requestType = table.Column<int>(type: "integer", nullable: false),
                    treatmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    isUrgent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRequest", x => x.requestId);
                    table.ForeignKey(
                        name: "FK_PatientRequest_Patients_patientId",
                        column: x => x.patientId,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_patientId",
                table: "Appointment",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_placeId",
                table: "Appointment",
                column: "placeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecord_patientId",
                table: "MedicalRecord",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRequest_patientId",
                table: "PatientRequest",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_patientAddressId",
                table: "Patients",
                column: "patientAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "MedicalRecord");

            migrationBuilder.DropTable(
                name: "PatientRequest");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
