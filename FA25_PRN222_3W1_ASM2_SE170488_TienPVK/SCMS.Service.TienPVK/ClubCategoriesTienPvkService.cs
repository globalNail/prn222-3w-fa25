using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK;

namespace SCMS.Service.TienPVK;

public class ClubCategoriesTienPvkService
{
    private readonly ClubCategoriesTienPvkRepository _repository;

    public ClubCategoriesTienPvkService() => _repository = new ClubCategoriesTienPvkRepository();

    public async Task<List<ClubCategoriesTienPvk>> GetAllAsync() => await _repository.GetAllAsync();
}
