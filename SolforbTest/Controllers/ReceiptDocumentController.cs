using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SolforbTest.Domain;
using SolforbTest.Services;
using SolforbTest.ViewModels;
using System.Linq;

namespace SolforbTest.Controllers
{
    public class ReceiptDocumentController : Controller
    {
        private readonly ReceiptDocumentService _service;
        private readonly ResourceService _resourceService;
        private readonly MeasurementUnitService _unitService;

        public ReceiptDocumentController(ReceiptDocumentService service, ResourceService resourceService, MeasurementUnitService unitService)
        {
            _service = service;
            _resourceService = resourceService;
            _unitService = unitService;
        }

        [HttpGet]
        public IActionResult Index(DateTime? dateFrom, DateTime? dateTo, List<string> numbers, List<int> resources, List<int> units)
        {
            var vm = new ReceiptDocumentIndexViewModel
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                SelectedNumbers = numbers ?? new List<string>(),
                SelectedResourceIds = resources ?? new List<int>(),
                SelectedMeasurementUnitIds = units ?? new List<int>()
            };

            // Опции должны быть независимы от периода
            vm.NumberOptions = _service.GetAllNumbers().Select(n => new SelectListItem { Value = n, Text = n, Selected = vm.SelectedNumbers.Contains(n) }).ToList();
            vm.ResourceOptions = _service.GetAllResources().Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name, Selected = vm.SelectedResourceIds.Contains(r.Id) }).ToList();
            vm.MeasurementUnitOptions = _service.GetAllMeasurementUnits().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name, Selected = vm.SelectedMeasurementUnitIds.Contains(u.Id) }).ToList();

            vm.Documents = _service.GetFiltered(dateFrom, dateTo, vm.SelectedNumbers, vm.SelectedResourceIds, vm.SelectedMeasurementUnitIds);

            return View(vm);
        }

        private void PopulateOptions(ReceiptDocumentViewModel vm)
        {
            vm.ResourceOptions = _resourceService.GetActive().Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList();
            vm.MeasurementUnitOptions = _unitService.GetActive().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
        }

        public IActionResult Create()
        {
            var vm = new ReceiptDocumentViewModel
            {
                Lines = new List<ReceiptResourceRowViewModel> { new ReceiptResourceRowViewModel() }
            };
            PopulateOptions(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReceiptDocumentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateOptions(vm);
                return View(vm);
            }

            var lines = (vm.Lines ?? new List<ReceiptResourceRowViewModel>())
                .Where(l => l.ResourceId.HasValue && l.MeasurementUnitId.HasValue)
                .Select(l => new ReceiptResource
                {
                    ResourceId = l.ResourceId!.Value,
                    MeasurementUnitId = l.MeasurementUnitId!.Value,
                    Quantity = l.Quantity
                }).ToList();

            try
            {
                _service.Add(vm.Number, vm.Date, lines);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Number", ex.Message);
                PopulateOptions(vm);
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var document = _service.GetWithResources(id);
            if (document == null)
                return NotFound();

            var vm = new ReceiptDocumentViewModel
            {
                Id = document.Id,
                Number = document.Number,
                Date = document.Date,
                Lines = document.ReceiptResources.Select(rr => new ReceiptResourceRowViewModel
                {
                    Id = rr.Id,
                    ResourceId = rr.ResourceId,
                    MeasurementUnitId = rr.MeasurementUnitId,
                    Quantity = (int)rr.Quantity
                }).ToList()
            };
            if (!vm.Lines.Any()) vm.Lines.Add(new ReceiptResourceRowViewModel());
            PopulateOptions(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReceiptDocumentViewModel vm)
        {
            if (!ModelState.IsValid || vm.Id == null)
            {
                PopulateOptions(vm);
                return View(vm);
            }

            var document = new ReceiptDocument
            {
                Id = vm.Id.Value,
                Number = vm.Number,
                Date = vm.Date
            };

            var lines = (vm.Lines ?? new List<ReceiptResourceRowViewModel>())
                .Where(l => l.ResourceId.HasValue && l.MeasurementUnitId.HasValue)
                .Select(l => new ReceiptResource
                {
                    Id = l.Id ?? 0,
                    ResourceId = l.ResourceId!.Value,
                    MeasurementUnitId = l.MeasurementUnitId!.Value,
                    Quantity = l.Quantity
                }).ToList();

            try
            {
                _service.UpdateWithResources(document, lines);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Number", ex.Message);
                PopulateOptions(vm);
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
