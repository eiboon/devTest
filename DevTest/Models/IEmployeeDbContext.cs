using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public interface IEmployeeDbContext
    {
        DbSet<Employee> Employees { get; set; }
        int SaveChanges();

    }
}