namespace SolforbTest.Domain
{
    public class ReceiptResource
    {
        public int Id { get; set; }
        public int ReceiptDocumentId { get; set; }
        public int ResourceId { get; set; }
        public int MeasurementUnitId { get; set; }
        public decimal Quantity { get; set; }

        // Навигационные свойства
        public ReceiptDocument ReceiptDocument { get; set; }
        public Resource Resource { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
    }
}

