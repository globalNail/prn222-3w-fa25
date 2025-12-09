using SCMS.Domain.TienPVK.Models;

namespace SCMS.Service.TienPVK.Interfaces;

public interface IClubCategoriesTienPvkService
{
    Task<List<ClubCategoriesTienPvk>> GetAllAsync();
}
