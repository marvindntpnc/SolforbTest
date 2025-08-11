using Microsoft.AspNetCore.Mvc;
using SolforbTest.Services;

namespace SolforbTest.Controllers
{
    public class ResourceController : Controller
    {
        private readonly ResourceService _service;

        public ResourceController(ResourceService service)
        {
            _service = service;
        }

        // GET: /Resource
        public IActionResult Index()
        {
            var resources = _service.GetAll();
            return View(resources);
        }

        // GET: /Resource/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Resource/Create
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

        // GET: /Resource/Edit/5
        public IActionResult Edit(int id)
        {
            var resource = _service.GetById(id);
            if (resource == null)
                return NotFound();
            return View(resource);
        }

        // POST: /Resource/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string name, bool isActive)
        {
            var resource = _service.GetById(id);
            if (resource == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Наименование обязательно");
                return View(resource);
            }
            resource.Name = name;
            resource.IsActive = isActive;
            _service.Update(resource);
            return RedirectToAction("Index");
        }

        // POST: /Resource/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }

        // POST: /Resource/ChangeState/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeState(int id, bool isActive)
        {
            _service.ChangeState(id, isActive);
            return RedirectToAction("Index");
        }
    }
}
