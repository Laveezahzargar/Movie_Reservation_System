using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P12_Movie_Reservation_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Mig_006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservedByUserId",
                table: "ShowSeats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservedUntil",
                table: "ShowSeats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_ReservedByUserId",
                table: "ShowSeats",
                column: "ReservedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowSeats_Users_ReservedByUserId",
                table: "ShowSeats",
                column: "ReservedByUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowSeats_Users_ReservedByUserId",
                table: "ShowSeats");

            migrationBuilder.DropIndex(
                name: "IX_ShowSeats_ReservedByUserId",
                table: "ShowSeats");

            migrationBuilder.DropColumn(
                name: "ReservedByUserId",
                table: "ShowSeats");

            migrationBuilder.DropColumn(
                name: "ReservedUntil",
                table: "ShowSeats");
        }
    }
}
