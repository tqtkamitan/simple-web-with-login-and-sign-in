using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Models
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
