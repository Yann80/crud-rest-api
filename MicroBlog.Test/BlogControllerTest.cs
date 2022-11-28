using MicroBlog.Controllers;
using MicroBlog.Models;
using MicroBlog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MicroBlog.Test
{
    [TestClass]
    public class BlogControllerTest
    {
        public BlogController mockController;
        private readonly Mock<IDataRepository<Blog>> mocRepo;

        public BlogControllerTest()
        {
            var BlogTestData = new List<Blog>() { new Blog { 
                BlogId = 1,
                Title = "test title",
                Body = "test body",
                AuthorId = 1,
                PublishedDate = DateTime.Now
            } };

            var blogs = MockDbSet(BlogTestData);
            blogs.Setup(a => a.FindAsync(It.IsAny<object[]>())).Returns((object[] obj) => {
                return new ValueTask<Blog>(BlogTestData.FirstOrDefault(b => b.BlogId == (int)obj[0]));
            });

            var dbContext = new Mock<IDataContext>();
            dbContext.Setup(m => m.Blogs).Returns(blogs.Object);
            dbContext.Setup(a => a.SetModified(It.IsAny<Blog>()));

            mocRepo = new Mock<IDataRepository<Blog>>();
            mockController = new BlogController(dbContext.Object,mocRepo.Object);
        }

        private static Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        [TestMethod]
        public void GetBlogsTest()
        {
            var result = mockController.GetBlogs();
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteBlogTest()
        {
            var id = 1;
            var result = await mockController.DeleteBlog(id);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateAuthorTest()
        {
            var result = await mockController.UpdateBlog(1, new Blog
            {
                BlogId = 1,
                Title = "test title",
                Body = "test body",
                AuthorId = 1,
                PublishedDate = DateTime.Now
            });
            var okResult = result as NoContentResult;
            Assert.IsInstanceOfType(okResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PostAuthorTest()
        {
            var result = await mockController.PostBlog(new Blog
            {
                BlogId = 1,
                Title = "test title",
                Body = "test body",
                AuthorId = 1,
                PublishedDate = DateTime.Now
            });
            var okResult = result as CreatedAtActionResult;
            Assert.IsInstanceOfType(okResult, typeof(CreatedAtActionResult));
        }
    }
}
