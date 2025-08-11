using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Services
{
    public class MeasurementUnitService
    {
        private readonly IMeasurementUnitRepository _repository;
        private readonly IMeasurementUnitFactory _factory;

        public MeasurementUnitService(IMeasurementUnitRepository repository, IMeasurementUnitFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public IEnumerable<MeasurementUnit> GetAll()
        {
            return _repository.GetAll();
        }

        public MeasurementUnit GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(string name, bool isActive = true)
        {
            var unit = _factory.Create(name, isActive);
            _repository.Add(unit);
        }

        public void Update(MeasurementUnit unit)
        {
            _repository.Update(unit);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public void ChangeState(int id, bool isActive)
        {
            var unit = _repository.GetById(id);
            if (unit != null)
            {
                unit.IsActive = isActive;
                _repository.Update(unit);
            }
        }
    }
}
