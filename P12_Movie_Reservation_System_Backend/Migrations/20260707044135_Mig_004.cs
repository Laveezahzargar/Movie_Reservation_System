using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P12_Movie_Reservation_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Mig_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Theaters_TheaterName_Location",
                table: "Theaters");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Theaters");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Theaters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Theaters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Theaters_CityId",
                table: "Theaters",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Theaters_TheaterName_Address",
                table: "Theaters",
                columns: new[] { "TheaterName", "Address" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Theaters_Cities_CityId",
                table: "Theaters",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Theaters_Cities_CityId",
                table: "Theaters");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Theaters_CityId",
                table: "Theaters");

            migrationBuilder.DropIndex(
                name: "IX_Theaters_TheaterName_Address",
                table: "Theaters");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Theaters");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Theaters");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Theaters",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Theaters_TheaterName_Location",
                table: "Theaters",
                columns: new[] { "TheaterName", "Location" },
                unique: true);
        }
    }
}
