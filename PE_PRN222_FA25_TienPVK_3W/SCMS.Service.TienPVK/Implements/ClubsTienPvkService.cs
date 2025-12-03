using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.Implements;
using SCMS.Service.TienPVK.Interfaces;

namespace SCMS.Service.TienPVK.Implements;

public class ClubsTienPvkService : IClubsTienPvkService
{
    private readonly ClubsTienPvkRepository _repository;

    public ClubsTienPvkService(ClubsTienPvkRepository repository) => _repository = repository;

    public async Task<int> CreateAsync(ClubsTienPvk club) => await _repository.CreateAsync(club);

    public async Task<List<ClubsTienPvk>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<ClubsTienPvk?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<int> UpdateAsync(ClubsTienPvk club) => await _repository.UpdateAsync(club);

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {

            var entity = await _repository.GetByIdAsync(id);
            return entity == null 
                ? throw new Exception($"Club with id {id} not found.") 
                : await _repository.RemoveAsync(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting club with id {id}: {ex.Message}");
        }
    }
}
