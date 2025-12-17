using Entity.Models;
using Repository;

namespace Service;

public class LionProfileService
{
    private readonly LionProfileRepository _repository;

    public LionProfileService() => _repository = new LionProfileRepository();

    public async Task<bool> AnyAsync(int id) => await _repository.AnyAsync(id);

    public async Task<int> CreateAsync(LionProfile entity) => await _repository.CreateAsync(entity);

    public async Task<List<LionProfile>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<LionProfile?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<int> UpdateAsync(LionProfile entity) => await _repository.UpdateAsync(entity);

    public async Task<List<LionProfile>> Search(string name, double weight) => await _repository.Search(name, weight);

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"Club with id {id} not found.");
            }

            await _repository.RemoveAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting entity with id {id}: {ex.Message}");
        }
    }
}
