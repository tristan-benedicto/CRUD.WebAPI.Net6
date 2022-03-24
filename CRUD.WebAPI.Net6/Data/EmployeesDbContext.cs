using CRUD.WebAPI.Net6.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.WebAPI.Net6.Data
{
    public class EmployeesDbContext : DbContext
    {
        public EmployeesDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

    }
}
