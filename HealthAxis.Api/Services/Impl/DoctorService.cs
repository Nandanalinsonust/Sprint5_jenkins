using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace HealthAxis.Api.Services.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        private readonly UserManager<ApplicationUser> userManager;
        public async Task<DoctorDto> AddAsync(CreateDoctorDto dto)
        {
            var doctor = mapper.Map<Doctor>(dto);
            doctor.IsActive = true;

            var saved = await repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(saved);
        }


        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            return mapper.Map<DoctorDto>(await repository.GetByIdAsync(id));
        }

        public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = mapper.Map<Doctor>(dto);
            doctor.DoctorId = id;

            var updated = await repository.UpdateAsync(id, doctor);
            return mapper.Map<DoctorDto>(updated);
        }

        public async Task<List<DoctorDto>> GetByNameAsync(string name)
        {
            var data = await repository.GetByNameAsync(name);
            return mapper.Map<List<DoctorDto>>(data);
        }


        public async Task<List<DoctorDto>> GetBySpecialisationAsync(DoctorSpecialisation specialization)
        {
            var data = await repository.GetBySpecialisationAsync(specialization.ToString());
            return mapper.Map<List<DoctorDto>>(data);
        }


        public async Task<object> GetAvailabilityAsync(int doctorId, DateTime date)
        {
            return await repository.GetAvailabilityAsync(doctorId, date);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            return await repository.DeactivateAsync(id);
        }
        public async Task AssignUserAsync(int doctorId, string userId)
        {
            var doctor = await repository.GetByIdAsync(doctorId);
            doctor.UserId = userId;
            await repository.UpdateAsync(doctorId, doctor);
        }
        public async Task<DoctorDto> GetByUserIdAsync(string userId)
        {
            var doctor = await repository.GetByUserIdAsync(userId);
            return mapper.Map<DoctorDto>(doctor);
        }
        public async Task<List<DoctorDto>> GetAllAsync(int page, int pageSize)
        {
            var data = await repository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(
                data.Skip((page - 1) * pageSize).Take(pageSize).ToList()
            );
        }
        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
{
    var user = await userManager.FindByIdAsync(userId);

    if (user == null)
        return false;

    var check = await userManager.CheckPasswordAsync(user, dto.CurrentPassword);

    if (!check)
        return false;

    var result = await userManager.ChangePasswordAsync(
        user,
        dto.CurrentPassword,
        dto.NewPassword
    );

    if (!result.Succeeded)
        return false;

    var doctor = await repository.GetByUserIdAsync(userId);
    doctor.IsFirstLogin = false;

    await repository.UpdateAsync(doctor.DoctorId, doctor);

    return true;
}

    }
}