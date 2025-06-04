using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Models;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace UAT.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);
            // Act
            var result = controller.Index();
            // Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);
            // Act
            var result = controller.Privacy();
            // Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void Error_ReturnsViewResultWithErrorViewModel()
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);
            // Act
            var result = controller.Error();

            Assert.IsType<ViewResult>(result);

            var viewResult = result as ViewResult;

            // Assert
            Assert.NotNull(viewResult);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.False(string.IsNullOrEmpty(((ErrorViewModel)viewResult.Model).RequestId));
        }
        [Fact]
        public void EmployeeSuccess_ReturnsViewResult()
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Role = "Developer",
                Email = "a@a.com",
                Phone = "1234567890"
            };
            // Act
            var result = controller.EmployeeForm(employee);
            // Assert
            Assert.IsType<ViewResult>(result);
            
            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
            Assert.NotNull(result.Model);

            var e = result.Model as List<Employee>;
            Assert.NotNull(e);
            Assert.Contains(employee, e);

        }
    }
}
