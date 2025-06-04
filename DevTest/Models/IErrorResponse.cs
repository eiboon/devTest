namespace Web.Models
{
    public interface IErrorResponse
    {
        string Error { get; set; }
        string Field { get; set; }

    }
}