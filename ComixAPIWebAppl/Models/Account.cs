using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComixAPIWebApp.Models
{
    public class Account
    {
        public Account()
        {
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(100)]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(150)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(100)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Роль")]
        public int RoleId { get; set; }

        [Display(Name = "Заблокований")]
        public bool IsBlocked { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual Role Role { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}