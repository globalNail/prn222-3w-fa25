using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

namespace Repository;

public class ChargingSessionRepository : GenericRepository<ChargingSession>
{
    public ChargingSessionRepository()
    {
    }

    public ChargingSessionRepository(FA25EChargingDBDbContext context) => _context = context;

    public new async Task<List<ChargingSession>> GetAllAsync() => await _context.ChargingSessions
        .Include(c => c.Station)
        .Include(c => c.Driver)
        .OrderByDescending(c => c.StartTime)
        .AsNoTracking()
        .ToListAsync();

    public new async Task<ChargingSession?> GetByIdAsync(int id) => await _context.ChargingSessions
        .Include(c => c.Station)
        .Include(c => c.Driver)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.SessionId == id );

    public async Task<bool> AnyAsync(int id)
    {
        var entity = await _context.ChargingSessions.AnyAsync(c => c.SessionId == id);
        if (entity)
        {
            return true;
        }
        return false;
    }
}
