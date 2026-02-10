using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_golek_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedReservationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CustomerId", "EndTime", "RoomId", "StartTime" },
                values: new object[] { 1, 1, new DateTime(2026, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 2, 10, 10, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
