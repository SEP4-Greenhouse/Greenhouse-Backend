using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCGreenhouse.Migrations
{
    /// <inheritdoc />
    public partial class AddSensorReadingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    HashedPassword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Greenhouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlantType = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Greenhouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Greenhouse_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Controller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    GreenhouseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Controller_Greenhouse_GreenhouseId",
                        column: x => x.GreenhouseId,
                        principalTable: "Greenhouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Species = table.Column<string>(type: "TEXT", nullable: false),
                    PlantingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GrowthStage = table.Column<string>(type: "TEXT", nullable: false),
                    GreenhouseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plant_Greenhouse_GreenhouseId",
                        column: x => x.GreenhouseId,
                        principalTable: "Greenhouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    GreenhouseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensor_Greenhouse_GreenhouseId",
                        column: x => x.GreenhouseId,
                        principalTable: "Greenhouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    ControllerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Controller_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    SensorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReadings_Sensor_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionAlert",
                columns: table => new
                {
                    TriggeredAlertsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TriggeringActionsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionAlert", x => new { x.TriggeredAlertsId, x.TriggeringActionsId });
                    table.ForeignKey(
                        name: "FK_ActionAlert_Action_TriggeringActionsId",
                        column: x => x.TriggeringActionsId,
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionAlert_Alert_TriggeredAlertsId",
                        column: x => x.TriggeredAlertsId,
                        principalTable: "Alert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlertSensorReading",
                columns: table => new
                {
                    TriggeredAlertsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TriggeringSensorReadingsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertSensorReading", x => new { x.TriggeredAlertsId, x.TriggeringSensorReadingsId });
                    table.ForeignKey(
                        name: "FK_AlertSensorReading_Alert_TriggeredAlertsId",
                        column: x => x.TriggeredAlertsId,
                        principalTable: "Alert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertSensorReading_SensorReadings_TriggeringSensorReadingsId",
                        column: x => x.TriggeringSensorReadingsId,
                        principalTable: "SensorReadings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantSensorReading",
                columns: table => new
                {
                    AffectedPlantsId = table.Column<int>(type: "INTEGER", nullable: false),
                    AffectingReadingsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantSensorReading", x => new { x.AffectedPlantsId, x.AffectingReadingsId });
                    table.ForeignKey(
                        name: "FK_PlantSensorReading_Plant_AffectedPlantsId",
                        column: x => x.AffectedPlantsId,
                        principalTable: "Plant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantSensorReading_SensorReadings_AffectingReadingsId",
                        column: x => x.AffectingReadingsId,
                        principalTable: "SensorReadings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ControllerId",
                table: "Action",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAlert_TriggeringActionsId",
                table: "ActionAlert",
                column: "TriggeringActionsId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertSensorReading_TriggeringSensorReadingsId",
                table: "AlertSensorReading",
                column: "TriggeringSensorReadingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Controller_GreenhouseId",
                table: "Controller",
                column: "GreenhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Greenhouse_UserId",
                table: "Greenhouse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plant_GreenhouseId",
                table: "Plant",
                column: "GreenhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantSensorReading_AffectingReadingsId",
                table: "PlantSensorReading",
                column: "AffectingReadingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_GreenhouseId",
                table: "Sensor",
                column: "GreenhouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_SensorId",
                table: "SensorReadings",
                column: "SensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionAlert");

            migrationBuilder.DropTable(
                name: "AlertSensorReading");

            migrationBuilder.DropTable(
                name: "PlantSensorReading");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropTable(
                name: "Plant");

            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "Controller");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "Greenhouse");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
