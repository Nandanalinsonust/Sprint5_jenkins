using HealthAxis.Api.Data;
using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories.Impl
{
    public class AppointmentRepository
        : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}