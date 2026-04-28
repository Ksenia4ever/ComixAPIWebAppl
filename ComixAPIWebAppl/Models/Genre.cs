using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComixAPIWebApp.Models
{
    public class Genre
    {
        public Genre()
        {
            Comics = new List<Comic>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(100)]
        [Display(Name = "Назва жанру")]
        public string Name { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual ICollection<Comic> Comics { get; set; }
    }
}