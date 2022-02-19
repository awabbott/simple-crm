
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyCrmApi.Migrations
{
    public partial class CoordinateDbSetAddedToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinate_Ships_ShipId",
                table: "Coordinate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coordinate",
                table: "Coordinate");

            migrationBuilder.RenameTable(
                name: "Coordinate",
                newName: "Coordinates");

            migrationBuilder.RenameIndex(
                name: "IX_Coordinate_ShipId",
                table: "Coordinates",
                newName: "IX_Coordinates_ShipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coordinates",
                table: "Coordinates",
                column: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_Ships_ShipId",
                table: "Coordinates",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "ShipId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_Ships_ShipId",
                table: "Coordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coordinates",
                table: "Coordinates");

            migrationBuilder.RenameTable(
                name: "Coordinates",
                newName: "Coordinate");

            migrationBuilder.RenameIndex(
                name: "IX_Coordinates_ShipId",
                table: "Coordinate",
                newName: "IX_Coordinate_ShipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coordinate",
                table: "Coordinate",
                column: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinate_Ships_ShipId",
                table: "Coordinate",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "ShipId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
