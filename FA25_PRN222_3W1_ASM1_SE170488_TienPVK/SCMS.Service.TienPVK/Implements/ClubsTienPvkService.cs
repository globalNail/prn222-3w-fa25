using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK;
using SCMS.Service.TienPVK.Interfaces;

namespace SCMS.Service.TienPVK.Implements;

public class ClubsTienPvkService : IClubsTienPvkService
{
    private readonly ClubsTienPvkRepository _repository;

    public ClubsTienPvkService() => _repository = new ClubsTienPvkRepository();

    public async Task<bool> AnyAsync(int categoryId) => await _repository.AnyAsync(categoryId);

    public async Task<int> CreateAsync(ClubsTienPvk club) => await _repository.CreateAsync(club);

    public async Task<List<ClubsTienPvk>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<ClubsTienPvk?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<int> UpdateAsync(ClubsTienPvk club) => await _repository.UpdateAsync(club);

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"Club with id {id} not found.");
            }

            // Soft delete: just mark as deleted instead of removing from database
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = "System"; // Or get from current user context

            await _repository.UpdateAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting club with id {id}: {ex.Message}");
        }
    }

    public async Task<List<ClubsTienPvk>> SearchAsync(string clubCode, string clubName, string status)
        => await _repository.SearchAsync(clubCode, clubName, status);
}
