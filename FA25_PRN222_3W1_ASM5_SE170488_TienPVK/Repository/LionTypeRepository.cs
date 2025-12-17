using Entity.Models;
using Repository.DBContext;

namespace Repository;

public class LionTypeRepository : GenericRepository<LionType>
{
    public LionTypeRepository()
    {
    }

    public LionTypeRepository(SU25LionDbContext context) => _context = context;
}
