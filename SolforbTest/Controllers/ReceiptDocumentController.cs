using Microsoft.AspNetCore.Mvc;
using SolforbTest.Domain;
using SolforbTest.Services;

namespace SolforbTest.Controllers
{
    public class ReceiptDocumentController : Controller
    {
        private readonly ReceiptDocumentService _service;

        public ReceiptDocumentController(ReceiptDocumentService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var documents = _service.GetAll();
            return View(documents);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string number, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                ModelState.AddModelError("Number", "Номер документа обязателен");
                return View();
            }
            _service.Add(number, date);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var document = _service.GetById(id);
            if (document == null)
                return NotFound();
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string number, DateTime date)
        {
            var document = _service.GetById(id);
            if (document == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(number))
            {
                ModelState.AddModelError("Number", "Номер документа обязателен");
                return View(document);
            }
            document.Number = number;
            document.Date = date;
            _service.Update(document);
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
