using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace MicroBlog.Models
{
    public interface IDataContext
    {
        EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        void SetModified(object entity);
    }
}
