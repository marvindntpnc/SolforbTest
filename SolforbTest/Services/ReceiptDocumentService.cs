using SolforbTest.Domain;
using SolforbTest.Interfaces;
using SolforbTest.Data;
using Microsoft.EntityFrameworkCore;

namespace SolforbTest.Services
{
    public class ReceiptDocumentService
    {
        private readonly IReceiptDocumentRepository _repository;
        private readonly IReceiptDocumentFactory _factory;
        private readonly ApplicationContext _context;

        public ReceiptDocumentService(IReceiptDocumentRepository repository, IReceiptDocumentFactory factory, ApplicationContext context)
        {
            _repository = repository;
            _factory = factory;
            _context = context;
        }

        public IEnumerable<ReceiptDocument> GetAll()
        {
            return _repository.GetAll();
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
            return _repository.GetById(id);
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

        public void Add(string number, DateTime date)
        {
            var document = _factory.Create(number, date);
            _repository.Add(document);
        }

        public void Add(string number, DateTime date, IEnumerable<ReceiptResource> resources)
        {
            var document = _factory.Create(number, date, resources);
            if (document.ReceiptResources != null)
            {
                foreach (var line in document.ReceiptResources)
                {
                    // Явно связываем строки с документом для корректной установки FK
                    line.ReceiptDocument = document;
                    line.ReceiptDocumentId = 0; // будет выставлен EF при сохранении
                    line.Id = 0; // гарантируем добавление как новых строк
                }
            }
            _repository.Add(document);
        }

        public void Update(ReceiptDocument document)
        {
            _repository.Update(document);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
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
