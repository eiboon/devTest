namespace Web.Models
{
    //Used for simple in-memory storage of Employee objects
    //This saves writing to DB before ready to implement persistence
    public class SimpleRepo
    {
        private static List<Employee> _employees = new();
        public static IEnumerable<Employee> Employees => _employees;
        public static void Add(Employee e)
        {
            _employees.Add(e);
        }
    }
}
