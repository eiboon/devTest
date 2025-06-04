namespace Web.Models
{
    //use default constructor as simple
    public class EmployeeErrorResponse(string field, string error) : IErrorResponse
    {
        public string Field { get; set; } = field;
        public string Error { get; set; } = error;
    }

}
