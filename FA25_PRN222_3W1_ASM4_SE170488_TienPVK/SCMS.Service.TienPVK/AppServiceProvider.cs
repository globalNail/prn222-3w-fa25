namespace SCMS.Service.TienPVK;

/// <summary>
/// Service provider implementation that provides centralized access to all services.
/// This follows the Facade Pattern, allowing access like: _service.ClubService.GetAllAsync()
/// </summary>
public class AppServiceProvider 
{
    public AppServiceProvider(
        ClubsTienPvkService clubService,
        ClubCategoriesTienPvkService categoryService,
        SystemAccountService accountService)
    {
        ClubService = clubService;
        CategoryService = categoryService;
        AccountService = accountService;
    }

    public ClubsTienPvkService ClubService { get; }
    
    public ClubCategoriesTienPvkService CategoryService { get; }
    
    public SystemAccountService AccountService { get; }
}
