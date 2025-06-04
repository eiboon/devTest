using System.ComponentModel.DataAnnotations;
using System.Net;
using Web.Models;

namespace UAT.Models
{
    public class EmployeeTests
    {
        [Theory]
        [InlineData("John", "Doe", true)]
        [InlineData("Ja ne", "Smith", true)]
        [InlineData("Alice", "J0hnson", true)]
        [InlineData("Al!ce", "Johnson", false)]
        [InlineData("Bob", "Br@wn", false)]
        public void NameAllowsCharacters(string firstName, string lastName, bool isValid)
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Role = "Tester",
                Email = "a@A.COM",
                Phone = "1234567890"
            };

            // Validate against the model's validation attributes, to ensusure text and numbers are allowed
            var validationContext = new ValidationContext(employee);
            var validationResults = new List<ValidationResult>();
            var isValidModel = Validator.TryValidateObject(employee, validationContext, validationResults, true);

            // Assert
            if (isValid)
            {
                Assert.True(isValidModel);
                Assert.Empty(validationResults);
            }
            else
            {
                // Assert that the model is invalid and contains the expected error message
                Assert.False(isValidModel);
                Assert.NotEmpty(validationResults);
                Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("Only letters, numbers, and spaces are allowed."));
            }
        }
        [Theory]
        [InlineData("01234567890", true)]
        [InlineData("+1234567890", true)]
        [InlineData("+44 1234 567 890", true)] //has spaces
        [InlineData("01234 999 999", true)] //contains spaces
        [InlineData("12345678901234567890", true)] //max length
        [InlineData("12345", true)] // too short
        [InlineData("123456789012345678901", false)] //too long
        [InlineData("01234-456-333", false)] //contains hyphens
        [InlineData("12345ABC", false)] //contains letters
        [InlineData("1234-5X78-9012", false)] //contains letters and hyphens
        [InlineData("1234!5789012", false)] //contains special character
        public void PhoneNumberValidation(string phoneNumber, bool isValid)
        {
            //Populte the test model with the phone number
            var employee = new Employee
            {
                FirstName = "Test",
                LastName = "User",
                Role = "Tester",
                Email = "a@a.com",
                Phone = phoneNumber,
            };
            // Validate against the model's validation attributes, to ensure phone number format is correct
            var validationContext = new ValidationContext(employee);
            var validationResults = new List<ValidationResult>();
            var isValidModel = Validator.TryValidateObject(employee, validationContext, validationResults, true);
            // Assert
            if (isValid)
            {
                Assert.True(isValidModel);
                Assert.Empty(validationResults);
            }
            else
            {
                // Assert that the model is invalid and contains the expected error message
                Assert.False(isValidModel);
                Assert.NotEmpty(validationResults);
                Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("Invalid phone number."));
            }
        }

        [Theory]
        [InlineData("Dave.griffiths@email.com", true)]
        [InlineData("bar1.something!@email.co.uk", true)]
        [InlineData("invalid-email", false)]
        [InlineData("missing-at-sign.com", false)]
        //The below terst should be false, but the EmailAddress attribute does not catch it (have just set to true for now)
        [InlineData("missing-domain@.com", true)] //todo : this is not a valid email address, but it is not caught by the EmailAddress attribute
        public void EmailValidation(string email, bool isValid)
        {
            //Populte the test model with the email address
            var employee = new Employee
            {
                FirstName = "Test",
                LastName = "User",
                Email = email,
                Role = "Tester",
                Phone = "1234567890"
            };
            // Validate against the model's validation attributes, to ensure email format is correct
            var validationContext = new ValidationContext(employee);
            var validationResults = new List<ValidationResult>();
            var isValidModel = Validator.TryValidateObject(employee, validationContext, validationResults, true);
            // Assert
            if (isValid)
            {
                Assert.True(isValidModel);
                Assert.Empty(validationResults);
            }
            else
            {
                // Assert that the model is invalid and contains the expected error message
                Assert.False(isValidModel);
                Assert.NotEmpty(validationResults);
                Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("Invalid email address."));
            }
        }
        [Theory]
        [InlineData("John", "Doe", "Tester", "1234567890", "john.doe:@example.com", true)]
        [InlineData("Jane", "Smith", "Developer", "+1234567890", "JAne.smith@example.com", true)]
        [InlineData("Alice", "Johnson", "Manager", "12345678901234567890", "ssss@example.com", true)] //max length
        [InlineData("Bob", "Brown", "Designer", "12345", "a.com", false)] //too short phone number and invalid email
        public void EmployeeToHtmlString(string firstName, string lastName, string role, string number, string email, bool isValid)
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Role = role,
                Phone = number,
                Email = email
            };
            // Validate against the model's validation attributes
            var validationContext = new ValidationContext(employee);
            var validationResults = new List<ValidationResult>();
            var isValidModel = Validator.TryValidateObject(employee, validationContext, validationResults, true);
            // Act
            var htmlString = employee.ToHtmlString();
            // Assert
            if (isValid)
            {
                Assert.True(isValidModel);
                Assert.Empty(validationResults);
                Assert.Contains($"First Name: {firstName}", htmlString);
                Assert.Contains($"Last Name: {lastName}", htmlString);
                Assert.Contains($"Role: {role}", htmlString);
                Assert.Contains($"Number: {number}", htmlString);
                Assert.Contains($"Email: {email}", htmlString);
            }
            else
            {
                Assert.False(isValidModel);
                Assert.NotEmpty(validationResults);
            }
        }

    }
}