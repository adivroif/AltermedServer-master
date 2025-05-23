using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltermedManager.Migrations
    {
    /// <inheritdoc />
    public partial class ChangeDateInSlotsToDateOnly : Migration
        {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
            {
            // 1. Temporarily rename the old column
            migrationBuilder.RenameColumn(
                name: "date_of_treatment",
                table: "AppointmentSlots",
                newName: "OldDate");

            // 2. Add the new Date column as DateOnly-compatible (i.e. SQL type `date`)
            migrationBuilder.AddColumn<DateOnly>(
                name: "date_of_treatment",
                table: "AppointmentSlots",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2000, 1, 1));  // default placeholder

            // 3. Migrate existing values using SQL (assumes dd/MM/yyyy format)
            migrationBuilder.Sql(@"
        UPDATE ""AppointmentSlots""
        SET ""date_of_treatment"" = TO_DATE(""OldDate"", 'DD/MM/YYYY')
        WHERE ""OldDate"" ~ '^[0-9]{2}/[0-9]{2}/[0-9]{4}$'
    ");

            // 4. Drop the old column
            migrationBuilder.DropColumn(
                name: "OldDate",
                table: "AppointmentSlots");
            }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
            {
            // 1. Re-add the old Date column as string (nvarchar)
            migrationBuilder.AddColumn<string>(
                name: "OldDate",
                table: "AppointmentSlots",
                type: "nvarchar(max)",
                nullable: true);

            // 2. Copy values back from the new DateOnly column to OldDate (as string in dd/MM/yyyy)
            migrationBuilder.Sql(@"
        UPDATE AppointmentSlots
        SET OldDate = FORMAT(date_of_treatment, 'dd/MM/yyyy')
    ");

            // 3. Drop the DateOnly column
            migrationBuilder.DropColumn(
                name: "date_of_treatment",
                table: "AppointmentSlots");

            // 4. Rename OldDate back to Date
            migrationBuilder.RenameColumn(
                name: "OldDate",
                table: "AppointmentSlots",
                newName: "Date");
            }
        }
    }
