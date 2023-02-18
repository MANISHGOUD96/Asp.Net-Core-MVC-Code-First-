using Microsoft.EntityFrameworkCore;
using MK_Core_MVC.Models;

namespace MK_Core_MVC.DB_Connection
{
    public class Table:DbContext
    {
        public Table(DbContextOptions<Table> options):base(options)
        {

        }
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<Login> Logins { get; set; }
    }
}
