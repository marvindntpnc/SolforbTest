using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Factories
{
    public class ResourceFactory : IResourceFactory
    {
        public Resource Create(string name, bool isActive = true)
        {
            return new Resource
            {
                Name = name,
                IsActive = isActive
            };
        }
    }
}
