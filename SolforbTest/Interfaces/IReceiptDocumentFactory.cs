using SolforbTest.Domain;

namespace SolforbTest.Interfaces
{
    public interface IReceiptDocumentFactory
    {
        ReceiptDocument Create(string number, DateTime date);
    }
}
