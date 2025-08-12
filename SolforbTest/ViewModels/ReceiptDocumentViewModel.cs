using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SolforbTest.ViewModels
{
    public class ReceiptDocumentViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Номер документа")]
        [Required]
        public string Number { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        public List<ReceiptResourceRowViewModel> Lines { get; set; } = new();

        public IEnumerable<SelectListItem> ResourceOptions { get; set; } = Array.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> MeasurementUnitOptions { get; set; } = Array.Empty<SelectListItem>();
    }
}

