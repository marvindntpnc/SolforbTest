using Microsoft.AspNetCore.Mvc;
using SolforbTest.Domain;
using SolforbTest.Services;

namespace SolforbTest.Controllers
{
    public class MeasurementUnitController : Controller
    {
        private readonly MeasurementUnitService _service;

        public MeasurementUnitController(MeasurementUnitService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var units = _service.GetAll();
            return View(units);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string name, bool isActive = true)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Наименование обязательно");
                return View();
            }
            _service.Add(name, isActive);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var unit = _service.GetById(id);
            if (unit == null)
                return NotFound();
            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string name, bool isActive)
        {
            var unit = _service.GetById(id);
            if (unit == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Наименование обязательно");
                return View(unit);
            }
            unit.Name = name;
            unit.IsActive = isActive;
            _service.Update(unit);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeState(int id, bool isActive)
        {
            _service.ChangeState(id, isActive);
            return RedirectToAction("Index");
        }
    }
}
