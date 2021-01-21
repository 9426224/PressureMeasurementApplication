using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PressureMeasurementApplication.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MissionModel",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SensorNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataModel",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MissionModelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataModel_MissionModel_MissionModelId",
                        column: x => x.MissionModelId,
                        principalTable: "MissionModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataModel_MissionModelId",
                table: "DataModel",
                column: "MissionModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataModel");

            migrationBuilder.DropTable(
                name: "MissionModel");
        }
    }
}
