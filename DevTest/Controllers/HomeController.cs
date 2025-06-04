using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeDbContext _context;


        public HomeController(ILogger<HomeController> logger, IEmployeeDbContext employeeDb)
        {
            _logger = logger;
            _context = employeeDb;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult EmployeeForm()
        {
            return View();
        }
        [HttpPost]
        public ViewResult EmployeeForm(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                //If the model state is not valid, return the view with the current model state
                return View();
            }
            else
            {
                //Save the reults to the database
                //TODO: Implement database save logic here
                _context.Employees.Add(employee);
       
                return View("Index", _context.Employees);
            }
        }
        private List<EmployeeErrorResponse> GenerateErrors()
        {
            Response.StatusCode = 500;
            //Build our error handling data to be sent back to page
            List<EmployeeErrorResponse> errorResponses = new();

            foreach (var error in ModelState)
            {
                //Just confirm we have errors, there should be
                if (error.Value.Errors.Count > 0)
                {
                    var errorResponse = new EmployeeErrorResponse(
                        error.Key, error.Value.Errors.First().ErrorMessage
                        );
                    errorResponses.Add(errorResponse);
                }
            }
            return errorResponses;
        }
        [HttpPost]
        public JsonResult EmployeeFormAjax(string firstname, string lastname, string role, string phone, string email)
        {

            // Validate the model state
            if (!ModelState.IsValid)
            {

                //ErrorResponses errorResponses = new (employeeErrorResponses);
                //Send our error messages back to the form
                return Json(GenerateErrors());
            }
            else //Success
            {
                // Create a new employee object from the form data
                var employee = new Employee
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Role = role,
                    Phone = phone,
                    Email = email
                };
                // Add the employee to the repository
                _context.Employees.Add(employee);
                try { 
                    // Save changes to the database
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    //TODO Duplicate email check, or other database related issues
                    //duplicate will return here it just needs to be handled correctly.

                    // Log the error and return a 500 status code
                    _logger.LogError(ex, "An error occurred while saving the employee.");
                    Response.StatusCode = 500;
                    return Json(GenerateErrors());
                }


                // Return a success response with the employee's HTML representation
                return Json(employee);
            }
        }
        [HttpPost]
        public JsonResult EmployeeQuickSearch(string text)
        {
            var response = null as IEnumerable<string>;
            //Fill the response with the employee names that match the search text
            if (_context.Employees.Any() && !string.IsNullOrEmpty(text))
            {
                //Oh my, we want specific results, and we dont want duplicate names
                //When we search we will get all names, as email is primary key
                var results = _context.Employees.AsQueryable().Where(
                    e => e.LastName.Contains(text) || e.FirstName.Contains(text)
                    ).ToArray<Employee>().Distinct();
                if (results != null)
                {
                    //return firstname lastname
                    response = results.Select(e => $"{e.FirstName} {e.LastName}");
                }
            }

            return Json(response);
        }
        [HttpPost]
        public JsonResult EmployeeSearch(string text)
        {
            var results = null as IEnumerable<Employee>;
            
            //Fill the response with the employee names that match the search text
            if (_context.Employees.Count() > 0 && !string.IsNullOrEmpty(text))
            {
                results = _context.Employees;
                var r = results.Where(e => e.FirstName.Contains(text) || e.LastName.Contains(text) || $"{e.FirstName} {e.LastName}".Contains(text));
                return Json(r);
            }

            return Json(results);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? "Unknown Error1";
            try
            {
                requestId = HttpContext.TraceIdentifier;
            }
            catch
            {
                //do nothing as this is an error from UAT project
            }
            return View(
                new ErrorViewModel
                {
                    RequestId = requestId
                });
        }
    }
}
