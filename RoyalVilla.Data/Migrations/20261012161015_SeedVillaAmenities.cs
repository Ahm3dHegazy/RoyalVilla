using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoyalVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VillaAmenities",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt", "VillaId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Heated outdoor pool with a sun deck and loungers.", "Private Pool", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-speed fiber internet available throughout the villa.", "Free Wi-Fi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Central air conditioning in all bedrooms and living areas.", "Air Conditioning", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panoramic sea view from the terrace and master bedroom.", "Sea View", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Outdoor barbecue area with seating for eight guests.", "BBQ Area", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Covered parking space for up to three cars.", "Private Parking", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fully equipped private gym with cardio and weight machines.", "Gym", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Landscaped garden with a seating area and fire pit.", "Garden", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "55-inch smart TV with streaming apps in the living room.", "Smart TV", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "VillaAmenities",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
