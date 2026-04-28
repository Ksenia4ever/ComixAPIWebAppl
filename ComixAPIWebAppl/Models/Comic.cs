using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComixAPIWebApp.Models
{
    public class Comic
    {
        public Comic()
        {
            ComicAuthors = new List<ComicAuthor>();
            CartItems = new List<CartItem>();
            OrderItems = new List<OrderItem>();
            Reviews = new List<Review>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(200)]
        [Display(Name = "Назва коміксу")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }

        [Display(Name = "Кількість на складі")]
        public int StockQuantity { get; set; }

        [Display(Name = "Рік випуску")]
        public int? ReleaseYear { get; set; }

        [Display(Name = "Видавництво")]
        public int PublisherId { get; set; }

        [Display(Name = "Жанр")]
        public int GenreId { get; set; }

        [StringLength(255)]
        [Display(Name = "Шлях до обкладинки")]
        public string? CoverImagePath { get; set; }

        [Display(Name = "Активний")]
        public bool IsActive { get; set; }

        [Display(Name = "Створено")]
        public DateTime Created { get; set; }

        [Display(Name = "Змінено")]
        public DateTime Modified { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual Genre Genre { get; set; }

        public virtual ICollection<ComicAuthor> ComicAuthors { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}