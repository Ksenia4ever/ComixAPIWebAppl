using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComixAPIWebApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Display(Name = "Кошик")]
        public int CartId { get; set; }

        [Display(Name = "Комікс")]
        public int ComicId { get; set; }

        [Display(Name = "Кількість")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Ціна за одиницю")]
        public decimal UnitPrice { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Comic Comic { get; set; }
    }
}