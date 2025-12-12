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
        .Include(c => c.CategoryIdtienPvkNavigation)
        .Include(c => c.ManagerUser)
        .Where(c => !c.IsDeleted)
        .OrderByDescending(c => c.CreatedAt)
        .AsNoTracking()
        .ToListAsync();

    public new async Task<ClubsTienPvk?> GetByIdAsync(int id) => await _context.ClubsTienPvks
        .Include(c => c.CategoryIdtienPvkNavigation)
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
}
