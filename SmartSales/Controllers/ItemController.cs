using Microsoft.AspNetCore.Mvc;
using SmartSales.Models;
using SmartSales.Repository.IRepository;

namespace SmartSales.Controllers
{
    public class ItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Item> objItemList = _unitOfWork.ItemRepository.GetAll().ToList();
            return View(objItemList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ItemRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Item Success Created";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
