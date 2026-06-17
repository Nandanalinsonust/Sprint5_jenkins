using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();


        }
    }
}
