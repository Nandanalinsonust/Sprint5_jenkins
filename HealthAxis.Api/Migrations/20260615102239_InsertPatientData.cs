using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthAxis.Api.Migrations
{
    /// <inheritdoc />

    public partial class InsertPatientData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "Email", "FullName", "Gender", "InsuranceID", "IsActive", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1998, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "nandana@example.com", "Nandana Linson", "Female", "INS1001", false, "9876543210" },
                    { 2, new DateTime(1990, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "arjun@example.com", "Arjun Menon", "Male", "INS1002", false, "9123456780" },
                    { 3, new DateTime(1985, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "meera@example.com", "Meera Nair", "Female", "INS1003", false, "9988776655" },
                    { 4, new DateTime(2000, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "rahul@example.com", "Rahul Das", "Male", "INS1004", false, "9012345678" },
                    { 5, new DateTime(1995, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "anjali@example.com", "Anjali Varma", "Female", "INS1005", false, "9345678901" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 5);
        }
    }
}
