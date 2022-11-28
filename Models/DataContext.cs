using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Models
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DataContext(DbContextOptions<DataContext> options): base(options) { }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
