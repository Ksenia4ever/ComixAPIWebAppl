using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComixAPIWebApp.Models
{
    public class Author
    {
        public Author()
        {
            ComicAuthors = new List<ComicAuthor>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(150)]
        [Display(Name = "ПІБ автора")]
        public string FullName { get; set; }

        [Display(Name = "Біографія")]
        public string? Biography { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual ICollection<ComicAuthor> ComicAuthors { get; set; }
    }
}