using SCMS.Domain.TienPVK.Models;

namespace SCMS.Service.TienPVK.Interfaces;

public interface IClubsTienPvkService
{
    Task<List<ClubsTienPvk>> GetAllAsync();
    Task<ClubsTienPvk?> GetByIdAsync(int id);
    Task<int> CreateAsync(ClubsTienPvk club);

    Task<int> UpdateAsync(ClubsTienPvk club);
    Task<bool> DeleteAsync(int id);
}
