using SolforbTest.Domain;
using SolforbTest.Interfaces;
using System.Linq;
using SolforbTest.Data;

namespace SolforbTest.Services
{
    public class MeasurementUnitService
    {
        private readonly IRepository<MeasurementUnit> _measureUnitRepository;
        private readonly IMeasurementUnitFactory _factory;
        private readonly ApplicationContext _context;

        public MeasurementUnitService(IRepository<MeasurementUnit> measureUnitRepository, IMeasurementUnitFactory factory, ApplicationContext context)
        {
            _measureUnitRepository = measureUnitRepository;
            _factory = factory;
            _context = context;
        }

        public IEnumerable<MeasurementUnit> GetAll()
        {
            return _measureUnitRepository.GetAll();
        }

        public IEnumerable<MeasurementUnit> GetActive()
        {
            return _measureUnitRepository.GetAll().Where(u => u.IsActive);
        }

        public MeasurementUnit GetById(int id)
        {
            return _measureUnitRepository.GetById(id);
        }

        public bool NameExists(string name, int? excludeId = null)
        {
            var query = _measureUnitRepository.GetAll().Where(u => u.Name.Trim().ToLower() == name.Trim().ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }
            return query.Any();
        }

        public void Add(string name, bool isActive = true)
        {
            if (NameExists(name))
            {
                throw new InvalidOperationException("Единица измерения с таким наименованием уже существует.");
            }
            var unit = _factory.Create(name, isActive);
            _measureUnitRepository.Add(unit);
        }

        public void Update(MeasurementUnit unit)
        {
            if (NameExists(unit.Name, unit.Id))
            {
                throw new InvalidOperationException("Единица измерения с таким наименованием уже существует.");
            }
            _measureUnitRepository.Update(unit);
        }

        public void Delete(int id)
        {
            var isUsed = _context.ReceiptResources.Any(rr => rr.MeasurementUnitId == id);
            if (isUsed)
            {
                throw new InvalidOperationException("Единица измерения используется в документах поступления и не может быть удалена. Переведите её в архив.");
            }
            _measureUnitRepository.Delete(id);
        }

        public void ChangeState(int id, bool isActive)
        {
            var unit = _measureUnitRepository.GetById(id);
            if (unit != null)
            {
                unit.IsActive = isActive;
                _measureUnitRepository.Update(unit);
            }
        }
    }
}
