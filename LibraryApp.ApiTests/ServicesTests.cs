using LibraryApp.API.Services;
using LibraryApp.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LibraryApp.ApiTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = LibraryAppData")
                .Options;
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);
                service.AddAuthor(new Data.Entities.Author()
                {
                    FirstName = "John",
                    LastName = "Cena"
                });
                service.Save();
            }
        }

        [Test]
        public void CreatingNewAuthor()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = LibraryAppData")
                .Options;
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);
                
                Assert.IsNotNull(context.Authors.Any(a => a.FirstName == "John"));
                Assert.IsNotNull(service.GetAuthor(1));
                Assert.IsTrue(service.AuthorExists(1));
            }
        }
    }
}