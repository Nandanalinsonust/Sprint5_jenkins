using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;

namespace HealthAxis.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();

            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientDto, Patient>();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();


        }
    }
}
