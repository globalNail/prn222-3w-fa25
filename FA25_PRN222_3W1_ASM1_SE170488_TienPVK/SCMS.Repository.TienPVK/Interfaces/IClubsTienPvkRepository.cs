using SCMS.Domain.TienPVK.Models;

namespace SCMS.Repository.TienPVK.Interfaces;

public interface IClubsTienPvkRepository : IGenericRepository<ClubsTienPvk>
{
    Task<IList<ClubsTienPvk>> GetAllAsync();
    Task<ClubsTienPvk?> GetByIdAsync(int id);
}
