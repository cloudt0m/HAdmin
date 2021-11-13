using Microsoft.EntityFrameworkCore;
using HAdmin.Models;

namespace HAdmin.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; init; }
    }
}