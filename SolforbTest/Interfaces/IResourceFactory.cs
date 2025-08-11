using SolforbTest.Domain;

namespace SolforbTest.Interfaces
{
    public interface IResourceFactory
    {
        Resource Create(string name, bool isActive = true);
    }
}
