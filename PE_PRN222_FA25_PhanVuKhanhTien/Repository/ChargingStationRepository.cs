using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

namespace Repository;

public class ChargingStationRepository : GenericRepository<ChargingStation>
{
    public ChargingStationRepository()
    {
    }

    public ChargingStationRepository(FA25EChargingDBDbContext context) => _context = context;
}
