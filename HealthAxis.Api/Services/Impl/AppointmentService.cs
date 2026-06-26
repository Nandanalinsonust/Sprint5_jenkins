using AutoMapper;
using HealthAxis.Api.Exceptions;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Api.Services.Impl
{
    public class AppointmentService(
        IAppointmentRepository repository,
        IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> AddAsync(CreateAppointmentDto dto)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            if (dto.ScheduledDate.Date < today)
                throw new InvalidException("Cannot book in the past");

            var slotStart = TimeSpan.Parse(dto.TimeSlot.Split('-')[0]);

            if (slotStart < TimeSpan.FromHours(9) || slotStart > TimeSpan.FromHours(17))
                throw new InvalidException("Outside working hours");

            if (dto.ScheduledDate.Date == today && slotStart <= now.TimeOfDay)
                throw new InvalidException("Invalid time slot");

            var patientAppointments = await repository.GetByPatientIdAsync(dto.PatientId);

            if (patientAppointments.Any(a =>
                a.TimeSlot == dto.TimeSlot &&
                a.ScheduledDate.Date == dto.ScheduledDate.Date))
                throw new InvalidException("Patient conflict");

            var doctorAppointments = await repository.GetByDoctorIdAsync(dto.DoctorId);

            if (doctorAppointments.Any(a =>
                a.TimeSlot == dto.TimeSlot &&
                a.ScheduledDate.Date == dto.ScheduledDate.Date))
                throw new InvalidException("Doctor conflict");

            var appointment = mapper.Map<Appointment>(dto);
            appointment.Status = "Pending";

            var saved = await repository.CreateAsync(appointment);

            return mapper.Map<AppointmentDto>(saved);
        }

        public async Task<bool> CancelWithValidation(int id)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity == null) return false;

            var slotStart = DateTime.Parse(entity.ScheduledDate.ToString("yyyy-MM-dd") + " " + entity.TimeSlot.Split('-')[0]);

            if ((slotStart - DateTime.Now).TotalHours < 2)
                throw new InvalidException("Cannot cancel within 2 hours");

            return await repository.DeleteAsync(id);
        }

        public async Task<List<AppointmentSummary>> GetSummaryReportAsync()
        {
            return await repository.GetSummaryReportAsync();
        }
        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();
            return mapper.Map<List<AppointmentDto>>(data);
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            var result = await repository.GetByIdAsync(id);
            return mapper.Map<AppointmentDto>(result);
        }

        public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            appointment.AppointmentId = id;
            var updated = await repository.UpdateAsync(id, appointment);
            return mapper.Map<AppointmentDto>(updated);
        }

        public async Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            var data = await repository.GetByDoctorIdAsync(doctorId);
            return mapper.Map<List<AppointmentDto>>(data);
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            var data = await repository.GetByPatientIdAsync(patientId);
            return mapper.Map<List<AppointmentDto>>(data);
        }


        public async Task<AppointmentDto> UpdateStatusAsync(int id, AppointmentStatus status)
        {
            var updated = await repository.UpdateStatusAsync(id, status.ToString());
            return mapper.Map<AppointmentDto>(updated);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            return await repository.DeleteAsync(id);
        }
        public async Task<bool> HasDoctorTreatedPatient(int doctorId, int patientId)
        {
            var appointments = await repository.GetByDoctorIdAsync(doctorId);

            return appointments.Any(a => a.PatientId == patientId);
        }

    }
}

