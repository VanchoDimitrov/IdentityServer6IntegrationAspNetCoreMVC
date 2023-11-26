using Integration.DataLayer.UnitOfWork;
using Integration.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace IS6IntegrationAspNetCoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            try
            {
                var categories = _unitOfWork.CategoryUoW.GetAll();
                return View(categories);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryUoW.Get(c => c.CategoryID == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CategoryUoW.Add(category);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryUoW.Get(c => c.CategoryID == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            try
            {
                if (id != category.CategoryID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _unitOfWork.CategoryUoW.Update(category);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryUoW.Get(c => c.CategoryID == id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryUoW.Get(c => c.CategoryID == id);
                if (category == null)
                {
                    return NotFound();
                }

                _unitOfWork.CategoryUoW.Delete(category);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error");
            }
        }
    }
}
