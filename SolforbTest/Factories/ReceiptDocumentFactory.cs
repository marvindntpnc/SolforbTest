using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Factories
{
    public class ReceiptDocumentFactory : IReceiptDocumentFactory
    {
        public ReceiptDocument Create(string number, DateTime date)
        {
            return new ReceiptDocument
            {
                Number = number,
                Date = date
            };
        }
    }
}
