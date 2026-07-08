using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthAxis.Api.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "HealthRecordId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "HealthRecordId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "HealthRecordId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "CreatedDate", "DoctorName", "Email", "IdentityUserId", "IsActive", "MustChangePassword", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 5000, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Arun Menon", "arun.menon@example.com", null, true, false, 9, 10 },
                    { 2, 1000, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Meera Nair", "meera.nair@example.com", null, true, false, 8, 15 },
                    { 3, 700, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Vikram Das", "vikram.das@example.com", null, true, false, 7, 8 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "CreatedDate", "DateOfBirth", "Email", "Gender", "IdentityUserId", "InsuranceID", "PatientName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1998, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "ravi.kumar@example.com", 0, null, "INS1001", "Ravi Kumar", "9876543210" },
                    { 2, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2001, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), "anjali.nair@example.com", 1, null, "INS1002", "Anjali Nair", "8765432109" },
                    { 3, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "kiran.das@example.com", 3, null, null, "Kiran Das", "7654321098" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "CancellationReason", "CreatedDate", "DoctorId", "PatientId", "ScheduledDate", "Status", "TimeSlot" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0, "10:00 AM - 10:30 AM" },
                    { 2, null, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, "11:00 AM - 11:30 AM" },
                    { 3, "Patient requested cancellation", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, 3, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, "02:00 PM - 02:30 PM" }
                });

            migrationBuilder.InsertData(
                table: "HealthRecords",
                columns: new[] { "HealthRecordId", "AppointmentId", "CreatedDate", "Diagnosis", "DoctorId", "Notes", "PatientId", "Prescription", "VisitDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Fever and cold", 1, "Drink enough water and take rest", 1, "Paracetamol 500mg twice daily", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Chest discomfort", 2, "Avoid heavy exercise until review", 2, "ECG test and follow-up consultation", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 3, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Consultation cancelled", null, "Appointment was cancelled by patient", 3, "No prescription issued", new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
