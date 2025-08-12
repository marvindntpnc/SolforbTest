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

        public IActionResult Index()
        {
            var documents = _service.GetAllWithResources();
            return View(documents);
        }

        private void PopulateOptions(ReceiptDocumentViewModel vm)
        {
            vm.ResourceOptions = _resourceService.GetAll().Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList();
            vm.MeasurementUnitOptions = _unitService.GetAll().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
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

            _service.Add(vm.Number, vm.Date, lines);
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

            _service.UpdateWithResources(document, lines);
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
