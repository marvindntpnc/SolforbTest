using SolforbTest.Domain;

namespace SolforbTest.Interfaces
{
    public interface IMeasurementUnitFactory
    {
        MeasurementUnit Create(string name, bool isActive = true);
    }
}
