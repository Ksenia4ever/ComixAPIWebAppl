using System;
using System.ComponentModel.DataAnnotations;

namespace ComixAPIWebApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Display(Name = "Комікс")]
        public int ComicId { get; set; }

        [Display(Name = "Користувач")]
        public int AccountId { get; set; }

        [Range(1, 5, ErrorMessage = "Оцінка повинна бути від 1 до 5")]
        [Display(Name = "Оцінка")]
        public int Rating { get; set; }

        [Display(Name = "Коментар")]
        public string? Comment { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual Comic Comic { get; set; }
        public virtual Account Account { get; set; }
    }
}