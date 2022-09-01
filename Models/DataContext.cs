using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace ListUsers.Models
{
   public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
