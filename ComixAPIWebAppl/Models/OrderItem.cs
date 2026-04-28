using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComixAPIWebApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Display(Name = "Замовлення")]
        public int OrderId { get; set; }

        [Display(Name = "Комікс")]
        public int ComicId { get; set; }

        [Display(Name = "Кількість")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Ціна за одиницю")]
        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual Comic Comic { get; set; }
    }
}