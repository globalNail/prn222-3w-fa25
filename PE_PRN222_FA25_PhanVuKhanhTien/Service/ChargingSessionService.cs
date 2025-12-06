using Entity.Models;
using Repository;

namespace Service;

public class ChargingSessionService
{
    private readonly ChargingSessionRepository _repository;

    public ChargingSessionService() => _repository = new ChargingSessionRepository();

    public async Task<bool> AnyAsync(int categoryId) => await _repository.AnyAsync(categoryId);

    public async Task<int> CreateAsync(ChargingSession club) => await _repository.CreateAsync(club);

    public async Task<List<ChargingSession>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<ChargingSession?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<int> UpdateAsync(ChargingSession club) => await _repository.UpdateAsync(club);

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is not null)
            {
                await _repository.RemoveAsync(entity);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting with id {id}: {ex.Message}");
        }
    }
}