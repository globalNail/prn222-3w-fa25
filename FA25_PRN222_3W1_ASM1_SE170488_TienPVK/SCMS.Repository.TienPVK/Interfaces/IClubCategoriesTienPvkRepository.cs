using SCMS.Domain.TienPVK.Models;

namespace SCMS.Repository.TienPVK.Interfaces;

public interface IClubCategoriesTienPvkRepository : IGenericRepository<ClubCategoriesTienPvk>
{
    Task<IList<ClubCategoriesTienPvk>> GetAllAsync();

}
