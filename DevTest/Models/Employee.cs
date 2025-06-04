using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Employee
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only letters, numbers, and spaces are allowed.")]
        public required string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only letters, numbers, and spaces are allowed.")]
        public required string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only letters, numbers, and spaces are allowed.")]
        public required string Role { get; set; }

        //todo : this does not catch all invalid phone numbers, but it is a good start
        [Required]
        [RegularExpression(@"^[\\+0-9][0-9\s]{4,19}$", ErrorMessage = "Invalid phone number.")]
        public required string Phone { get; set; }

        // Email address validation using the built-in EmailAddress attribute
        // todo : this does not catch all invalid email addresses, but it is a good start
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

    }
}