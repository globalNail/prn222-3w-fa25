using Entity.Models;
using Repository;

namespace Service
{
    public class LionTypeService
	{
        private readonly LionTypeRepository _repository;

        public LionTypeService() => _repository ??= new LionTypeRepository();

        public LionTypeService(LionTypeRepository repository) => _repository = repository;

        public async Task<List<LionType>> GetAllAsync() => await _repository.GetAllAsync();

    }
}
