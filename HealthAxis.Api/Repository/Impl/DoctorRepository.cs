using HealthAxis.Api.Data;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repository.Interface;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Repository.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly HealthAxisDbContext context;

        public DoctorRepository(HealthAxisDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<List<Doctor>> GetAllActiveAsync(
            CancellationToken ct = default)
        {
            return await context.Doctors
                .Where(doctor => doctor.IsActive)
                .ToListAsync(ct);
        }

        public async Task<List<Doctor>> GetBySpecialisationAsync(
            SpecialisationType specialisation,
            CancellationToken ct = default)
        {
            return await context.Doctors
                .Where(doctor => doctor.Specialisation == specialisation)
                .ToListAsync(ct);
        }

        public async Task<List<Doctor>> GetActiveBySpecialisationAsync(
            SpecialisationType specialisation,
            CancellationToken ct = default)
        {
            return await context.Doctors
                .Where(doctor =>
                    doctor.IsActive &&
                    doctor.Specialisation == specialisation)
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken ct = default)
        {
            return await context.Doctors
                .AnyAsync(
                    doctor => doctor.Email == email,
                    ct);
        }

        public async Task<Doctor?> GetByIdentityUserIdAsync(
            string identityUserId,
            CancellationToken ct = default)
        {
            return await context.Doctors
                .FirstOrDefaultAsync(
                    doctor => doctor.IdentityUserId == identityUserId,
                    ct);
        }
    }
}