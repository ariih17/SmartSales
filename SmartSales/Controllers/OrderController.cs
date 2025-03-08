using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartSales.Data;
using SmartSales.Models;
using SmartSales.Models.ViewModel;
using SmartSales.Repository.IRepository;

namespace SmartSales.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Order> objOrder = _unitOfWork.OrderRepository.GetAll(includeProperties: "Customer").ToList();

            return View(objOrder);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CustomerList = _unitOfWork.CustomerRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.CUSTOMER_NAME,
                Value = x.COM_CUSTOMER_ID.ToString()
            });

            //ViewBag.CustomerList = CustomerList;

            OrderViewModel orderVM = new()
            {
                CustomerList = CustomerList,
                orderList = new Order()
            };

            return View(orderVM);
        }

        [HttpPost]
        public IActionResult Create(OrderViewModel objOrderDM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderRepository.Add(objOrderDM.orderList);
                _unitOfWork.Save();
                TempData["success"] = "Order Success Created";
                return RedirectToAction("Edit", new { id = objOrderDM.orderList.SO_ORDER_ID });
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Order? orderFromDb = _unitOfWork.OrderRepository.Get(x => x.SO_ORDER_ID == id);
            if (orderFromDb == null) return NotFound();

            //munculin customer
            IEnumerable<SelectListItem> CustomerList = _unitOfWork.CustomerRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.CUSTOMER_NAME,
                Value = x.COM_CUSTOMER_ID.ToString()
            });

            //ViewBag.CustomerList = CustomerList;

            List<Order> itemList = _unitOfWork.OrderRepository.GetAll(includeProperties: "Items").Where(x => x.SO_ORDER_ID == id).ToList();
            List<Item> getAllItems = itemList.Select(x => x.Items).ToList();

            OrderViewModel orderVM = new()
            {
                CustomerList = CustomerList,
                orderList = orderFromDb,
                itemList = getAllItems
            };

            return View(orderVM);
        }

        [HttpPost]
        public IActionResult Edit(OrderViewModel objOrder)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderRepository.Update(objOrder.orderList);

                if (objOrder.itemList is IEnumerable<Item> items)
                {
                    foreach (var item in items)
                    {
                    //    var trackedItem = _unitOfWork.ItemRepository
                    //.GetAll() // Mengambil data tanpa melacak perubahan
                    //.FirstOrDefault(x => x.SO_ITEM_ID == item.SO_ITEM_ID);
                        item.SO_ORDER_ID = objOrder.orderList.SO_ORDER_ID;
                        if (item.SO_ITEM_ID == 0)
                        {   
                            _unitOfWork.ItemRepository.Add(item);
                        }
                        else
                        {
                            _unitOfWork.ItemRepository.Update(item);
                        }
                    }
                }

                _unitOfWork.Save();
                TempData["success"] = "Order Success Updated";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
