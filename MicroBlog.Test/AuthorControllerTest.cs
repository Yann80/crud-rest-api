using MicroBlog.Controllers;
using MicroBlog.Models;
using MicroBlog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MicroBlog.Test
{
    [TestClass]
    public class AuthorControllerTest
    {
        private AuthorController mockController;
        private readonly Mock<IDataRepository<Author>> mocRepo;

        public AuthorControllerTest()
        {
            var authorTestData = new List<Author>() { new Author { 
                AuthorId = 1,
                FirstName = "Mock user first name",
                LastName = "Mock user last name",
                Email = "email@mock.com",
                CreationDate = DateTime.Now
            } };

            var authors = MockDbSet(authorTestData);
            authors.Setup(a => a.FindAsync(It.IsAny<object[]>())).Returns((object[] obj) => {
                return new ValueTask<Author>(authorTestData.FirstOrDefault(b => b.AuthorId == (int)obj[0]));
            });

            var dbContext = new Mock<IDataContext>();
            dbContext.Setup(m => m.Authors).Returns(authors.Object);
            dbContext.Setup(a => a.SetModified(It.IsAny<Author>()));

            mocRepo = new Mock<IDataRepository<Author>>();
            mockController = new AuthorController(dbContext.Object,mocRepo.Object);
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
        public void GetAuthorsTest()
        {
            var result = mockController.GetAuthors();
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task DeleteAuthorTest()
        {
            var id = 1;
            var result = await mockController.DeleteAuthor(id);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UpdateAuthorTest()
        {
            var result = await mockController.UpdateAuthor(1,new Author
            {
                AuthorId = 1,
                FirstName = "Mock user first name1",
                LastName = "Mock user last name",
                Email = "email@mock.com",
                CreationDate = DateTime.Now
            });
            var okResult = result as NoContentResult;
            Assert.IsInstanceOfType(okResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PostAuthorTest()
        {
            var result = await mockController.PostAuthor(new Author
            {
                AuthorId = 1,
                FirstName = "Mock user first name1",
                LastName = "Mock user last name1",
                Email = "email@mock.com",
                CreationDate = DateTime.Now
            });
            var okResult = result as CreatedAtActionResult;
            Assert.IsInstanceOfType(okResult, typeof(CreatedAtActionResult));
        }
    }
}
