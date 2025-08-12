using SolforbTest.Domain;
using SolforbTest.Interfaces;

namespace SolforbTest.Factories
{
    public class ReceiptDocumentFactory : IReceiptDocumentFactory
    {
        public ReceiptDocument Create(string number, DateTime date, IEnumerable<ReceiptResource> resources = null)
        {
            var document = new ReceiptDocument
            {
                Number = number,
                Date = date
            };

            if (resources != null)
            {
                foreach (var resource in resources)
                {
                    document.ReceiptResources.Add(resource);
                }
            }

            return document;
        }
    }
}
