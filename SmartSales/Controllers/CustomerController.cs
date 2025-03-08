using Microsoft.AspNetCore.Mvc;
using SmartSales.Data;
using SmartSales.Models;
using SmartSales.Repository.IRepository;

namespace SmartSales.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Customer> objCustomerList = _unitOfWork.CustomerRepository.GetAll().ToList();
            return View(objCustomerList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer obj)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CustomerRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Success Created";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
