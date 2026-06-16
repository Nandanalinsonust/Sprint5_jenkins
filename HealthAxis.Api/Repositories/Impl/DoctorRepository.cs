using HealthAxis.Api.Data;
using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories.Impl
{
    public class DoctorRepository : Repository<Doctor> , IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
