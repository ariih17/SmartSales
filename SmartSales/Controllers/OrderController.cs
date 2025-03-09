using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClosedXML.Excel;
using SmartSales.Data;
using SmartSales.Models;
using SmartSales.Models.ViewModel;
using SmartSales.Repository.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet]
        public IActionResult GetAll(string query, string orderDate)
        {

            if (string.IsNullOrEmpty(query) && string.IsNullOrEmpty(orderDate))
            {
                List<Order> objOrderList = _unitOfWork.OrderRepository.GetAll(includeProperties: "Customer").ToList();
                var simplified = objOrderList.Select(x => new
                {
                    x.SO_ORDER_ID,
                    x.ORDER_NO,
                    x.ORDER_DATE,
                    x.CUSTOMER_NAME
                }).ToList();

                return Json(new { data = simplified });
            }
            else 
            {
                DateTime parseDate = DateTime.MinValue;
                if (orderDate != null)
                {
                   parseDate = DateTime.Parse(orderDate);
                }

                //List<Order> objSearch = _unitOfWork.OrderRepository.GetAll(includeProperties: "Customer")
                //.Where(x => x.ORDER_NO.Contains(query))
                //.Where(x => x.ORDER_DATE == parseDate).ToList();

                List<Order> objSearch = _unitOfWork.OrderRepository.GetAll(includeProperties: "Customer")
                .Where(x => x.ORDER_NO.Contains(query) || x.ORDER_DATE == parseDate).ToList();

                var simplifiedSearch = objSearch.Select(x => new
                {
                    x.SO_ORDER_ID,
                    x.ORDER_NO,
                    x.ORDER_DATE,
                    x.CUSTOMER_NAME
                }).ToList();

                return Json(new { data = simplifiedSearch });
            } 
        }

        public IActionResult ExportToExcel(string query, string orderDate)
        {
            List<Order> objOrderList;

            if (string.IsNullOrEmpty(query) && string.IsNullOrEmpty(orderDate))
            {
                objOrderList = _unitOfWork.OrderRepository.GetAll(includeProperties: "Customer").ToList();
            }
            else
            {
                var parseDate = DateTime.Parse(orderDate);

                objOrderList = _unitOfWork.OrderRepository.GetAll(includeProperties: "Customer")
                    .Where(x => x.ORDER_NO.Contains(query))
                    .Where(x => x.ORDER_DATE.Date == parseDate.Date)
                    .ToList();
            }

            // Membuat file Excel dengan ClosedXML
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");

                // Menambahkan header ke worksheet
                worksheet.Cell(1, 1).Value = "SO Order ID";
                worksheet.Cell(1, 2).Value = "Order No";
                worksheet.Cell(1, 3).Value = "Order Date";
                worksheet.Cell(1, 4).Value = "Customer Name";

                // Mengisi data
                for (int i = 0; i < objOrderList.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = objOrderList[i].SO_ORDER_ID;
                    worksheet.Cell(i + 2, 2).Value = objOrderList[i].ORDER_NO;
                    worksheet.Cell(i + 2, 3).Value = objOrderList[i].ORDER_DATE;
                    worksheet.Cell(i + 2, 4).Value = objOrderList[i].CUSTOMER_NAME;
                }

                // Simpan dan kembalikan file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
                }
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var getOrdertDelete = _unitOfWork.OrderRepository.Get(x => x.SO_ORDER_ID == id);
            if (getOrdertDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.OrderRepository.Remove(getOrdertDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}
