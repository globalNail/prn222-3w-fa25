using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.Implements;
using SCMS.Service.TienPVK.Interfaces;

namespace SCMS.Service.TienPVK.Implements;

public class ClubCategoriesTienPvkService : IClubCategoriesTienPvkService
{
    private readonly ClubCategoriesTienPvkRepository _repository;

    public ClubCategoriesTienPvkService() => _repository = new ClubCategoriesTienPvkRepository();

    public async Task<List<ClubCategoriesTienPvk>> GetAllAsync() => await _repository.GetAllAsync();
}
