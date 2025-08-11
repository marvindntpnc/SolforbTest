using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Services
{
    public class ResourceService
    {
        private readonly IResourceRepository _repository;
        private readonly IResourceFactory _factory;

        public ResourceService(IResourceRepository repository, IResourceFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public IEnumerable<Resource> GetAll()
        {
            return _repository.GetAll();
        }

        public Resource GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(string name, bool isActive = true)
        {
            var resource = _factory.Create(name, isActive);
            _repository.Add(resource);
        }

        public void Update(Resource resource)
        {
            _repository.Update(resource);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public void ChangeState(int id, bool isActive)
        {
            var resource = _repository.GetById(id);
            if (resource != null)
            {
                resource.IsActive = isActive;
                _repository.Update(resource);
            }
        }
    }
}
