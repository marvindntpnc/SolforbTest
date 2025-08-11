using SolforbTest.Domain;

namespace SolforbTest.Services
{
    public class ResourceFactory
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
