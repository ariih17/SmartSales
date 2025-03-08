using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SmartSales.Models
{
    public class Item
    {
        [Key]
        public long SO_ITEM_ID { get; set; }
        [ValidateNever]
        public long? SO_ORDER_ID { get; set; }
        [ValidateNever]
        public string? ITEM_NAME { get; set; }
        [ValidateNever]
        public int? QUANTITY { get; set; }
        [ValidateNever]
        public double? PRICE { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
