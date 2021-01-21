using Microsoft.EntityFrameworkCore.Migrations;

namespace PressureMeasurementApplication.Migrations
{
    public partial class ModifyDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "DataModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "DataModel");
        }
    }
}
