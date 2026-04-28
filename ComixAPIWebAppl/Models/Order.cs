using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComixAPIWebApp.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public int Id { get; set; }

        [Display(Name = "Користувач")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(50)]
        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Загальна сума")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}