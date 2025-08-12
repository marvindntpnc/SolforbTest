using System.ComponentModel.DataAnnotations;

namespace SolforbTest.ViewModels
{
    public class ReceiptResourceRowViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Ресурс")]
        [Required]
        public int? ResourceId { get; set; }

        [Display(Name = "Единица измерения")]
        [Required]
        public int? MeasurementUnitId { get; set; }

        [Display(Name = "Количество")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}

