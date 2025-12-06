using Entity.Models;
using Repository;

namespace Service;

public class ChargingStationService
{
    private readonly ChargingStationRepository _repository;

    public ChargingStationService() => _repository = new ChargingStationRepository();

    public async Task<List<ChargingStation>> GetAllAsync() => await _repository.GetAllAsync();
}
