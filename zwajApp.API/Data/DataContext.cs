using Microsoft.EntityFrameworkCore;
using zwajApp.API.Models;
using ZwajApp.API.Models;

namespace ZwajApp.API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options){}
        public DbSet<Value> Values {get; set;}
        public DbSet<Users> Users {get; set;}

    }
}