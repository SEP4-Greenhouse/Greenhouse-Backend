using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCGreenhouse.Migrations
{
    /// <inheritdoc />
    public partial class AddPrimaryKeyToPredictionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredictionTimestamp",
                table: "PredictionLogs");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PredictionLogs");

            migrationBuilder.RenameColumn(
                name: "SensorType",
                table: "PredictionLogs",
                newName: "TrendAnalysis");

            migrationBuilder.RenameColumn(
                name: "SensorTimestamp",
                table: "PredictionLogs",
                newName: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrendAnalysis",
                table: "PredictionLogs",
                newName: "SensorType");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "PredictionLogs",
                newName: "SensorTimestamp");

            migrationBuilder.AddColumn<DateTime>(
                name: "PredictionTimestamp",
                table: "PredictionLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "PredictionLogs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
