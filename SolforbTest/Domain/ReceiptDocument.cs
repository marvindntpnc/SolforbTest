namespace SolforbTest.Domain
{
    public class ReceiptDocument
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ReceiptResource> ReceiptResources { get; set; } = new List<ReceiptResource>();
    }
}
