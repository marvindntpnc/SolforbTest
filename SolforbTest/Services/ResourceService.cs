using SolforbTest.Domain;
using SolforbTest.Interfaces;
using System.Linq;
using SolforbTest.Data;

namespace SolforbTest.Services
{
    public class ResourceService
    {
        private readonly IRepository<Resource> _resourceRepository;
        private readonly IResourceFactory _factory;
        private readonly ApplicationContext _context;

        public ResourceService(IRepository<Resource> resourceRepository, IResourceFactory factory, ApplicationContext context)
        {
            _resourceRepository = resourceRepository;
            _factory = factory;
            _context = context;
        }

        public IEnumerable<Resource> GetAll()
        {
            return _resourceRepository.GetAll();
        }

        public IEnumerable<Resource> GetActive()
        {
            return _resourceRepository.GetAll().Where(r => r.IsActive);
        }

        public Resource GetById(int id)
        {
            return _resourceRepository.GetById(id);
        }

        public bool NameExists(string name, int? excludeId = null)
        {
            var query = _resourceRepository.GetAll().Where(r => r.Name.Trim().ToLower() == name.Trim().ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(r => r.Id != excludeId.Value);
            }
            return query.Any();
        }

        public void Add(string name, bool isActive = true)
        {
            if (NameExists(name))
            {
                throw new InvalidOperationException("Ресурс с таким наименованием уже существует.");
            }
            var resource = _factory.Create(name, isActive);
            _resourceRepository.Add(resource);
        }

        public void Update(Resource resource)
        {
            if (NameExists(resource.Name, resource.Id))
            {
                throw new InvalidOperationException("Ресурс с таким наименованием уже существует.");
            }
            _resourceRepository.Update(resource);
        }

        public void Delete(int id)
        {
            var isUsed = _context.ReceiptResources.Any(rr => rr.ResourceId == id);
            if (isUsed)
            {
                throw new InvalidOperationException("Ресурс используется в документах поступления и не может быть удалён. Переведите его в архив.");
            }
            _resourceRepository.Delete(id);
        }

        public void ChangeState(int id, bool isActive)
        {
            var resource = _resourceRepository.GetById(id);
            if (resource != null)
            {
                resource.IsActive = isActive;
                _resourceRepository.Update(resource);
            }
        }
    }
}
