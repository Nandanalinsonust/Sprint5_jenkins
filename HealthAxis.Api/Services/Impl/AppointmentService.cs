using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;

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
                throw new Exception("Cannot book appointment in the past");

            if (dto.ScheduledDate.Date == today)
            {
                var slotStart = TimeSpan.Parse(dto.TimeSlot.Split('-')[0]);
                if (slotStart <= now.TimeOfDay)
                    throw new Exception("Cannot book past time slot for today");
            }

            var patientAppointments = await repository.GetByPatientIdAsync(dto.PatientId);

            if (patientAppointments.Any(a =>
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate.Date == dto.ScheduledDate.Date))
            {
                throw new Exception("You already booked this doctor for the selected date");
            }

            if (patientAppointments.Any(a =>
                a.TimeSlot == dto.TimeSlot &&
                a.ScheduledDate.Date == dto.ScheduledDate.Date))
            {
                throw new Exception("You already have an appointment at this time");
            }

            var doctorAppointments = await repository.GetByDoctorIdAsync(dto.DoctorId);

            if (doctorAppointments.Any(a =>
                a.TimeSlot == dto.TimeSlot &&
                a.ScheduledDate.Date == dto.ScheduledDate.Date))
            {
                throw new Exception("Doctor is not available at this time slot");
            }

            var appointment = mapper.Map<Appointment>(dto);
            appointment.Status = "Pending";

            var saved = await repository.CreateAsync(appointment);

            return mapper.Map<AppointmentDto>(saved);
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

        public async Task<AppointmentDto> UpdateStatusAsync(int id, string status)
        {
            var updated = await repository.UpdateStatusAsync(id, status);
            return mapper.Map<AppointmentDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await repository.DeleteAsync(id);
        }

        public async Task<object> GetSummaryReportAsync()
        {
            return await repository.GetSummaryReportAsync();
        }
    }
}
