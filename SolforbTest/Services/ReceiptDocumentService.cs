using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Services
{
    public class ReceiptDocumentService
    {
        private readonly IReceiptDocumentRepository _repository;
        private readonly IReceiptDocumentFactory _factory;

        public ReceiptDocumentService(IReceiptDocumentRepository repository, IReceiptDocumentFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public IEnumerable<ReceiptDocument> GetAll()
        {
            return _repository.GetAll();
        }

        public ReceiptDocument GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(string number, DateTime date)
        {
            var document = _factory.Create(number, date);
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
    }
}
