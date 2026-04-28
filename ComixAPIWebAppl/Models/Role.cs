using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ComixAPIWebApp.Models
{
    public class Role
    {
        public Role()
        {
            Accounts = new List<Account>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва ролі")]
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}