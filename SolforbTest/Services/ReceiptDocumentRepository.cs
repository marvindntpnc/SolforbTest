using SolforbTest.Data;
using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Services
{
    public class ReceiptDocumentRepository : Repository<ReceiptDocument>, IReceiptDocumentRepository
    {
        public ReceiptDocumentRepository(ApplicationContext context) : base(context) { }
        // Специфичная логика для ReceiptDocument при необходимости
    }
}
