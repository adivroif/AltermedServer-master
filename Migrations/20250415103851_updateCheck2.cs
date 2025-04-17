using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class updateCheck2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    recommendationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patientId = table.Column<Guid>(type: "uuid", nullable: false),
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    recommendedTreatmentId = table.Column<int>(type: "integer", nullable: false),
                    recommendationDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false),
                    isChosen = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.recommendationId);
                    table.ForeignKey(
                        name: "FK_Recommendations_Appointments_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointments",
                        principalColumn: "appointmentId");
                    table.ForeignKey(
                        name: "FK_Recommendations_Patients_patientId",
                        column: x => x.patientId,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Treatments_recommendedTreatmentId",
                        column: x => x.recommendedTreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "treatmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_appointmentId",
                table: "Recommendations",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_patientId",
                table: "Recommendations",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_recommendedTreatmentId",
                table: "Recommendations",
                column: "recommendedTreatmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
