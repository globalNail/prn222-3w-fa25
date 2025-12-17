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
}
