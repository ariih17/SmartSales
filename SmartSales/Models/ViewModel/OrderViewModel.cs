using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartSales.Models.ViewModel
{
    public class OrderViewModel
    {
        public Order orderList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        [ValidateNever]
        public List<Item> itemList { get; set; }
        //public List<Order>? orderItems { get; set; }
    }
}
