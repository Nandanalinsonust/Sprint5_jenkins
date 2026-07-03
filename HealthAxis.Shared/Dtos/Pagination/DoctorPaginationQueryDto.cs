using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.Dtos.Pagination
{
    public class DoctorPaginationQueryDto : PaginationQueryDto
    {
        public SpecialisationType? Specialisation { get; set; }

        public bool? IsActive { get; set; }

        public string? SearchTerm { get; set; }
    }
}