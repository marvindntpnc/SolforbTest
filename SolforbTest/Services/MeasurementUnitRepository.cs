using SolforbTest.Data;
using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Services
{
    public class MeasurementUnitRepository : Repository<MeasurementUnit>, IMeasurementUnitRepository
    {
        public MeasurementUnitRepository(ApplicationContext context) : base(context) { }
        // Специфичная логика для MeasurementUnit при необходимости
    }
}
