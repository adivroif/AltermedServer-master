using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class updateTreatmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "numOfFeedbacks",
                table: "Treatments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "score",
                table: "Treatments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                name: "numOfFeedbacks",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "score",
                table: "Treatments");
        }
    }
}
