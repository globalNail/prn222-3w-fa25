using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.DBContext;

namespace SCMS.Repository.TienPVK;

public class ClubsTienPvkRepository : GenericRepository<ClubsTienPvk>
{
    public ClubsTienPvkRepository()
    {
    }

    public ClubsTienPvkRepository(FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext context) => _context = context;

    public new async Task<List<ClubsTienPvk>> GetAllAsync() => await _context.ClubsTienPvks
        .Include(c => c.Category)
        .Include(c => c.ManagerUser)
        .Where(c => !c.IsDeleted)
        .OrderByDescending(c => c!.ModifiedAt.HasValue
                                ? c.ModifiedAt.Value
                                : c.CreatedAt)
        .AsNoTracking()
        .ToListAsync();

    public new async Task<ClubsTienPvk?> GetByIdAsync(int id) => await _context.ClubsTienPvks
        .Include(c => c.Category)
        .Include(c => c.ManagerUser)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.ClubIdtienPvk == id && !c.IsDeleted);

    public async Task<bool> AnyAsync(int id)
    {
        var entity = await _context.ClubsTienPvks.AnyAsync(c => c.ClubIdtienPvk == id);
        if (entity)
        {
            return true;
        }
        return false;
    }

    public async Task<List<ClubsTienPvk>> SearchAsync(string clubCode, string clubName, string status)
    {
        var query = _context.ClubsTienPvks
            .Include(c => c.Category)
            .Include(c => c.ManagerUser)
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        // Apply search filters (OR operator - any match returns results)
        var hasAnyFilter = !string.IsNullOrWhiteSpace(clubCode) || 
                          !string.IsNullOrWhiteSpace(clubName) || 
                          !string.IsNullOrWhiteSpace(status);

        if (hasAnyFilter)
        {
            query = query.Where(c =>
                (!string.IsNullOrWhiteSpace(clubCode) && c.ClubCode != null && c.ClubCode.ToLower().Contains(clubCode.ToLower())) ||
                (!string.IsNullOrWhiteSpace(clubName) && c.ClubName != null && c.ClubName.ToLower().Contains(clubName.ToLower())) ||
                (!string.IsNullOrWhiteSpace(status) && c.Status != null && c.Status.ToLower().Contains(status.ToLower()))
            );
        }

        return await query
            .OrderByDescending(c => c!.ModifiedAt.HasValue ? c.ModifiedAt.Value : c.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }
}
