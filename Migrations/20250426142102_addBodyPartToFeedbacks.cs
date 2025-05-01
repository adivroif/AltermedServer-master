using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
{
    /// <inheritdoc />
    public partial class addBodyPartToFeedbacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            
            migrationBuilder.AddColumn<string>(
                name: "bodyPart",
                table: "PatientFeedbacks",
                type: "text",
                nullable: false,
                defaultValue: "");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "bodyPart",
                table: "PatientFeedbacks");

            
        }
    }
}
