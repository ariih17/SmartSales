using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace SmartSales.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SO_ORDER_ID { get; set; }
        [Required]
        public string ORDER_NO { get; set; }
        [Required]
        public DateTime ORDER_DATE { get; set; }
        [Required]
        public int COM_CUSTOMER_ID { get; set; }
        [Required]
        public string? ADDRESS { get; set; }

        //join customer
        [ValidateNever]   
        public virtual Customer Customer { get; set; }
        [ValidateNever]
        [NotMapped]
        public string CUSTOMER_NAME => Customer?.CUSTOMER_NAME;

        //join item
        [ValidateNever]
        public virtual Item Items { get; set; }
        //[ValidateNever]
        //[NotMapped]
        //public long? ORDER_ID => Item?.SO_ORDER_ID;
    }
}
