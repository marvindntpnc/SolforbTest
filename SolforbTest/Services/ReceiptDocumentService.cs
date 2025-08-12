using SolforbTest.Domain;
using SolforbTest.Interfaces;
using SolforbTest.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SolforbTest.Services
{
    public class ReceiptDocumentService
    {
        private readonly IRepository<ReceiptDocument> _receiptDocumentRepository;
        private readonly IReceiptDocumentFactory _factory;
        private readonly ApplicationContext _context;

        public ReceiptDocumentService(IRepository<ReceiptDocument> receiptDocumentRepository, IReceiptDocumentFactory factory, ApplicationContext context)
        {
            _receiptDocumentRepository = receiptDocumentRepository;
            _factory = factory;
            _context = context;
        }

        public IEnumerable<ReceiptDocument> GetAll()
        {
            return _receiptDocumentRepository.GetAll();
        }

        public IEnumerable<string> GetAllNumbers()
        {
            return _context.ReceiptDocuments
                .Select(d => d.Number)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }

        public IEnumerable<Resource> GetAllResources()
        {
            return _context.Set<Resource>().OrderBy(r => r.Name).ToList();
        }

        public IEnumerable<MeasurementUnit> GetAllMeasurementUnits()
        {
            return _context.Set<MeasurementUnit>().OrderBy(u => u.Name).ToList();
        }

        public IEnumerable<ReceiptDocument> GetFiltered(
            DateTime? dateFrom,
            DateTime? dateTo,
            IEnumerable<string> numbers,
            IEnumerable<int> resourceIds,
            IEnumerable<int> unitIds)
        {
            var query = _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                .ThenInclude(rr => rr.Resource)
                .Include(d => d.ReceiptResources)
                .ThenInclude(rr => rr.MeasurementUnit)
                .AsQueryable();

            if (dateFrom.HasValue)
            {
                query = query.Where(d => d.Date >= dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                query = query.Where(d => d.Date <= dateTo.Value);
            }
            if (numbers != null && numbers.Any())
            {
                var normalized = numbers.Select(n => n.Trim().ToLower()).ToList();
                query = query.Where(d => normalized.Contains(d.Number.ToLower()));
            }
            if (resourceIds != null && resourceIds.Any())
            {
                query = query.Where(d => d.ReceiptResources.Any(rr => resourceIds.Contains(rr.ResourceId)));
            }
            if (unitIds != null && unitIds.Any())
            {
                query = query.Where(d => d.ReceiptResources.Any(rr => unitIds.Contains(rr.MeasurementUnitId)));
            }

            return query
                .OrderByDescending(d => d.Date)
                .ThenBy(d => d.Number)
                .ToList();
        }

        public IEnumerable<ReceiptDocument> GetAllWithResources()
        {
            return _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                .ThenInclude(rr => rr.Resource)
                .Include(d => d.ReceiptResources)
                .ThenInclude(rr => rr.MeasurementUnit)
                .ToList();
        }

        public ReceiptDocument GetById(int id)
        {
            return _receiptDocumentRepository.GetById(id);
        }

        public ReceiptDocument GetWithResources(int id)
        {
            return _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                .ThenInclude(rr => rr.Resource)
                .Include(d => d.ReceiptResources)
                .ThenInclude(rr => rr.MeasurementUnit)
                .FirstOrDefault(d => d.Id == id);
        }

        public bool NumberExists(string number, int? excludeId = null)
        {
            var normalized = number.Trim().ToLower();
            var query = _context.ReceiptDocuments.AsQueryable().Where(d => d.Number.ToLower() == normalized);
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }
            return query.Any();
        }

        public void Add(string number, DateTime date)
        {
            if (NumberExists(number))
            {
                throw new InvalidOperationException("Документ с таким номером уже существует.");
            }
            var document = _factory.Create(number, date);
            _receiptDocumentRepository.Add(document);
        }

        public void Add(string number, DateTime date, IEnumerable<ReceiptResource> resources)
        {
            if (NumberExists(number))
            {
                throw new InvalidOperationException("Документ с таким номером уже существует.");
            }
            var document = _factory.Create(number, date, resources);
            if (document.ReceiptResources != null)
            {
                foreach (var line in document.ReceiptResources)
                {
                    line.ReceiptDocument = document;
                    line.ReceiptDocumentId = 0;
                    line.Id = 0;
                }
            }
            _receiptDocumentRepository.Add(document);
        }

        public void Update(ReceiptDocument document)
        {
            if (NumberExists(document.Number, document.Id))
            {
                throw new InvalidOperationException("Документ с таким номером уже существует.");
            }
            _receiptDocumentRepository.Update(document);
        }

        public void Delete(int id)
        {
            _receiptDocumentRepository.Delete(id);
        }

        public void AddResource(int documentId, int resourceId, int measurementUnitId, int quantity)
        {
            var document = _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                .FirstOrDefault(d => d.Id == documentId);
            if (document == null)
            {
                return;
            }

            var line = new ReceiptResource
            {
                ReceiptDocumentId = documentId,
                ResourceId = resourceId,
                MeasurementUnitId = measurementUnitId,
                Quantity = quantity
            };

            document.ReceiptResources.Add(line);
            _context.SaveChanges();
        }

        public void RemoveResource(int documentId, int receiptResourceId)
        {
            var document = _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                .FirstOrDefault(d => d.Id == documentId);
            if (document == null)
            {
                return;
            }

            var line = document.ReceiptResources.FirstOrDefault(r => r.Id == receiptResourceId);
            if (line != null)
            {
                document.ReceiptResources.Remove(line);
                _context.SaveChanges();
            }
        }

        public void UpdateWithResources(ReceiptDocument updatedDocument, IEnumerable<ReceiptResource> resources)
        {
            var existing = _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                .FirstOrDefault(d => d.Id == updatedDocument.Id);
            if (existing == null)
            {
                return;
            }

            if (NumberExists(updatedDocument.Number, updatedDocument.Id))
            {
                throw new InvalidOperationException("Документ с таким номером уже существует.");
            }

            existing.Number = updatedDocument.Number;
            existing.Date = updatedDocument.Date;

            existing.ReceiptResources.Clear();
            if (resources != null)
            {
                foreach (var resource in resources)
                {
                    existing.ReceiptResources.Add(new ReceiptResource
                    {
                        ReceiptDocumentId = existing.Id,
                        ResourceId = resource.ResourceId,
                        MeasurementUnitId = resource.MeasurementUnitId,
                        Quantity = resource.Quantity
                    });
                }
            }

            _context.SaveChanges();
        }
    }
}
