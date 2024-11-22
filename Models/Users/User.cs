using CodeExcercises.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CodeExcercises.Models.Users
{
    public class User : IUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Cellphone Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Cellphone Number must be exactly 10 digits")]
        public string CellphoneNumber { get; set; }
    }
}
