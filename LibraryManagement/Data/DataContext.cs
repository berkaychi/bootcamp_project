using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();

        public DbSet<User> Users => Set<User>();
    }
}