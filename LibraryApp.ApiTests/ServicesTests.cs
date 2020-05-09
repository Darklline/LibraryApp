using LibraryApp.API.Services;
using LibraryApp.Data.DbContexts;
using LibraryApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;

namespace LibraryApp.ApiTests
{
    public class Tests
    {
        private DbContextOptions<LibraryContext> options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = LibraryAppData")
                .Options;

        [SetUp]
        public void Setup()
        {
            using (var context = new LibraryContext(options))
            {
                context.Authors.AddRange(new Data.Entities.Author()
                {
                    FirstName = "John",
                    LastName = "Cena"
                },
                new Author()
                {
                    FirstName = "Rendy",
                    LastName = "Orton"
                },
                new Author()
                {
                    FirstName = "Mark",
                    LastName = "Anton"
                });
                context.SaveChanges();
            }
        }

        [Test]
        public void AddAuthor()
        {
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);
                service.AddAuthor(new Data.Entities.Author()
                {
                    FirstName = "Mark",
                    LastName = "Asd"
                });
                service.Save();

                Assert.IsNotNull(context.Authors.Any(a => a.FirstName == "John" && a.LastName == "Asd"));
            }
        }

        [Test]
        public void GetAuthor()
        {
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);

                Assert.IsNull(service.GetAuthor(0));
                Assert.IsNotNull(service.GetAuthor(1));
            }
        }

        [Test]
        public void AuthorExists()
        {
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);

                Assert.IsTrue(service.AuthorExists(1));
                Assert.IsFalse(service.AuthorExists(0));
            }
        }
        [Test]
        public void GetAuthors()
        {
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);

                var act = service.GetAuthors();

                Assert.IsNotNull(act);
                Assert.IsInstanceOf<IEnumerable>(act);
            }
        }

        [Test]
        public void DeleteAuthor()
        {
            using (var context = new LibraryContext(options))
            {
                var service = new LibraryRepository(context);
                var author = new Author() { Id = 3 };

                Assert.IsTrue(context.Authors.Any(a => a.Id == 3));
                service.DeleteAuthor(author);
                context.SaveChanges();
                Assert.IsFalse(context.Authors.Any(a => a.Id == 3));
            }
        }
    }
}