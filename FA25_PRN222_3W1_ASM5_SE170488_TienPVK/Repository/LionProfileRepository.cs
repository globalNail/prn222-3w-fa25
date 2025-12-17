using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

namespace Repository;

public class LionProfileRepository : GenericRepository<LionProfile>
{
    public LionProfileRepository()
    {
    }

    public LionProfileRepository(SU25LionDbContext context) => _context = context;

    public new async Task<List<LionProfile>> GetAllAsync() => await _context.LionProfiles
        .Include(c => c.LionType)
        .OrderByDescending(c => c.ModifiedDate)
        .AsNoTracking()
        .ToListAsync();

    public new async Task<LionProfile?> GetByIdAsync(int id) => await _context.LionProfiles
        .Include(c => c.LionType)
        .OrderByDescending(c => c.ModifiedDate)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.LionProfileId == id );

    public async Task<bool> AnyAsync(int id)
    {
        var entity = await _context.LionProfiles.AnyAsync(c => c.LionProfileId == id);
        if (entity)
        {
            return true;
        }
        return false;
    }

    public async Task<List<LionProfile>> Search(string name, double? weight)
    {
        var query = _context.LionProfiles.Include(a => a.LionType).AsQueryable();

        // OR logic: filter by weight OR lionTypeName
        if (!string.IsNullOrEmpty(name) || (weight.HasValue && weight.Value > 0))
        {
            query = query.Where(a => 
                (!string.IsNullOrEmpty(name) && a.LionType.LionTypeName.Contains(name)) ||
                (weight.HasValue && weight.Value > 0 && a.Weight == weight.Value)
            );
        }

        return await query.OrderByDescending(a => a.ModifiedDate).ToListAsync();
    }
}
