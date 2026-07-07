using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P12_Movie_Reservation_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Mig_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shows_ScreenId_ShowDateTime",
                table: "Shows");

            migrationBuilder.AlterColumn<int>(
                name: "ScreenId",
                table: "Shows",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Shows",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TheaterId",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScreenId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TheaterId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ScreenId_ShowDateTime",
                table: "Shows",
                columns: new[] { "ScreenId", "ShowDateTime" },
                unique: true,
                filter: "[ScreenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_TheaterId",
                table: "Shows",
                column: "TheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MovieId",
                table: "Bookings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ScreenId",
                table: "Bookings",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TheaterId",
                table: "Bookings",
                column: "TheaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Screens_ScreenId",
                table: "Bookings",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "ScreenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Theaters_TheaterId",
                table: "Bookings",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "TheaterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Theaters_TheaterId",
                table: "Shows",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "TheaterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Screens_ScreenId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Theaters_TheaterId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Theaters_TheaterId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ScreenId_ShowDateTime",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_TheaterId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MovieId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ScreenId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TheaterId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ScreenId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "ScreenId",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ScreenId_ShowDateTime",
                table: "Shows",
                columns: new[] { "ScreenId", "ShowDateTime" },
                unique: true);
        }
    }
}
