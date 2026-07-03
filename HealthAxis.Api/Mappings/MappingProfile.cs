using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src => Enum.Parse<DoctorSpecialisation>(src.Specialisation)));

            CreateMap<CreateDoctorDto, Doctor>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src => src.Specialisation.ToString()));

            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.Specialisation,
                    opt => opt.MapFrom(src => src.Specialisation.ToString()));

            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientDto, Patient>();

            CreateMap<Appointment, AppointmentDto>()
    .ForMember(
        dest => dest.Status,
        opt => opt.MapFrom(
            src => Enum.Parse<AppointmentStatus>(src.Status)
        ))
    .ForMember(
        dest => dest.PatientName,
        opt => opt.MapFrom(src => src.Patient.FullName)
        )
    .ForMember(
        dest => dest.DoctorName,
        opt => opt.MapFrom(src => src.Doctor.FullName)
        );

            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();

            CreateMap<CreateHealthRecordDto, HealthRecord>();
        }
    }
}