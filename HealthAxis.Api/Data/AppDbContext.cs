using HealthAxis.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>()
                .HasData(
                new Doctor { DoctorId = 1, FullName = "Nandana", Specialisation = "Cardiology", ConsultationFee = 500, YearsOfExperience = 3, IsActive = true },
                new Doctor { DoctorId = 2, FullName = "Arun Kumar", Specialisation = "Dermatology", ConsultationFee = 400, YearsOfExperience = 5, IsActive = true },
                new Doctor { DoctorId = 3, FullName = "Meera Joseph", Specialisation = "Pediatrics", ConsultationFee = 600, YearsOfExperience = 7, IsActive = true },
                new Doctor { DoctorId = 4, FullName = "Rahul Menon", Specialisation = "Orthopedics", ConsultationFee = 550, YearsOfExperience = 4, IsActive = true },
                new Doctor { DoctorId = 5, FullName = "Anjali Nair", Specialisation = "Neurology", ConsultationFee = 800, YearsOfExperience = 10, IsActive = true },
                new Doctor { DoctorId = 6, FullName = "Sandeep Varma", Specialisation = "General Medicine", ConsultationFee = 300, YearsOfExperience = 2, IsActive = true }
                );
            modelBuilder.Entity<Patient>()
                    .HasData(
                    new Patient { PatientId = 1, FullName = "Nandana Linson", DateOfBirth = new DateTime(1998, 5, 12), Gender = "Female", PhoneNumber = "9876543210", Email = "nandana@example.com", InsuranceID = "INS1001" },
                    new Patient { PatientId = 2, FullName = "Arjun Menon", DateOfBirth = new DateTime(1990, 8, 25), Gender = "Male", PhoneNumber = "9123456780", Email = "arjun@example.com", InsuranceID = "INS1002" },
                    new Patient { PatientId = 3, FullName = "Meera Nair", DateOfBirth = new DateTime(1985, 3, 10), Gender = "Female", PhoneNumber = "9988776655", Email = "meera@example.com", InsuranceID = "INS1003" },
                    new Patient { PatientId = 4, FullName = "Rahul Das", DateOfBirth = new DateTime(2000, 11, 2), Gender = "Male", PhoneNumber = "9012345678", Email = "rahul@example.com", InsuranceID = "INS1004" },
                    new Patient { PatientId = 5, FullName = "Anjali Varma", DateOfBirth = new DateTime(1995, 1, 18), Gender = "Female", PhoneNumber = "9345678901", Email = "anjali@example.com", InsuranceID = "INS1005" }
                    );
        }
    }
}