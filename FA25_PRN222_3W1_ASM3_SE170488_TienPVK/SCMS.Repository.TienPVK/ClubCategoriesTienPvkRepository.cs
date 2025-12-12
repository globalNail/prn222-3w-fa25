using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.DBContext;


namespace SCMS.Repository.TienPVK;

public class ClubCategoriesTienPvkRepository : GenericRepository<ClubCategoriesTienPvk>
{
    public ClubCategoriesTienPvkRepository()
    {
    }

    public ClubCategoriesTienPvkRepository(FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext context) : base(context)
    {
    }
}
