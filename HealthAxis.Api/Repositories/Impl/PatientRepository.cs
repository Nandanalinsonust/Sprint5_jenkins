using HealthAxis.Api.Data;
using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
        }
    }
}