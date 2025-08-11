using SolforbTest.Data;
using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Services
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        public ResourceRepository(ApplicationContext context) : base(context) { }
        // Здесь можно добавить специфичную логику для ресурсов при необходимости
    }
}
