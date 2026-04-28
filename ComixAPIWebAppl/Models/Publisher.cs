using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComixAPIWebApp.Models
{
    public class Publisher
    {
        public Publisher()
        {
            Comics = new List<Comic>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(150)]
        [Display(Name = "Назва видавництва")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "Країна")]
        public string? Country { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual ICollection<Comic> Comics { get; set; }
    }
}