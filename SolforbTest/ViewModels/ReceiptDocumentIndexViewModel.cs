using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SolforbTest.Domain;

namespace SolforbTest.ViewModels
{
    public class ReceiptDocumentIndexViewModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public List<string> SelectedNumbers { get; set; } = new();
        public List<int> SelectedResourceIds { get; set; } = new();
        public List<int> SelectedMeasurementUnitIds { get; set; } = new();

        public IEnumerable<SelectListItem> NumberOptions { get; set; } = Array.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ResourceOptions { get; set; } = Array.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> MeasurementUnitOptions { get; set; } = Array.Empty<SelectListItem>();

        public IEnumerable<ReceiptDocument> Documents { get; set; } = Array.Empty<ReceiptDocument>();
    }
}
