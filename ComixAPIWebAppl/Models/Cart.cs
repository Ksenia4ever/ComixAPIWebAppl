using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComixAPIWebApp.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public int Id { get; set; }

        [Display(Name = "Користувач")]
        public int AccountId { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}