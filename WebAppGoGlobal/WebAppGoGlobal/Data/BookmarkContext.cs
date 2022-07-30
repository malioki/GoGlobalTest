using Microsoft.EntityFrameworkCore;
using WebAppGoGlobal.Models;

namespace WebAppGoGlobal.Data
{
    public class BookmarkContext  : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public BookmarkContext(DbContextOptions<BookmarkContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
