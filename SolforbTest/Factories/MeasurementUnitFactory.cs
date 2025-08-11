using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Factories
{
    public class MeasurementUnitFactory : IMeasurementUnitFactory
    {
        public MeasurementUnit Create(string name, bool isActive = true)
        {
            return new MeasurementUnit
            {
                Name = name,
                IsActive = isActive
            };
        }
    }
}
