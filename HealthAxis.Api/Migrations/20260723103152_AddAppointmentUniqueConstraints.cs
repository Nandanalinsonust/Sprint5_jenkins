using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxis.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_ScheduledDate_TimeSlot",
                table: "Appointments",
                columns: new[] { "DoctorId", "ScheduledDate", "TimeSlot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId_ScheduledDate_TimeSlot",
                table: "Appointments",
                columns: new[] { "PatientId", "ScheduledDate", "TimeSlot" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_ScheduledDate_TimeSlot",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId_ScheduledDate_TimeSlot",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");
        }
    }
}
