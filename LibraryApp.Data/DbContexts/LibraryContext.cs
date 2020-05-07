using LibraryApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data.DbContexts
{
    public class LibraryContext: DbContext
    {
        public LibraryContext()
        {

        }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<BookType> BookTypes { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentBook>().HasKey(s => new { s.StudentId, s.BookId });
            //dummy data
            modelBuilder.Entity<Author>().HasData(
                new Author()
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Caston",
                },
                new Author()
                {
                    Id = 2,
                    FirstName = "Kate",
                    LastName = "Spenser"
                },
                new Author()
                {
                    Id = 3,
                    FirstName = "Jimmy",
                    LastName = "Slim"
                },
                new Author()
                {
                    Id = 4,
                    FirstName = "Sebastian",
                    LastName = "Jey"
                },
                new Author()
                {
                    Id = 5,
                    FirstName = "Adrian",
                    LastName = "Lek"
                },
                new Author()
                {
                    Id = 6,
                    FirstName = "Simon",
                    LastName = "Baed"
                },
                new Author()
                {
                    Id = 7,
                    FirstName = "Benjamin",
                    LastName = "Pear"
                },
                new Author()
                {
                    Id = 8,
                    FirstName = "Max",
                    LastName = "Vila"
                },
                new Author()
                {
                    Id = 9,
                    FirstName = "David",
                    LastName = "Beck"
                },
                new Author()
                {
                    Id = 10,
                    FirstName = "Patrick",
                    LastName = "Aleor"
                });

            modelBuilder.Entity<Book>().HasData(
                new Book()
                {
                    Id = 1,
                    Name = "Harry Potter",
                    PageCount = 500,
                    AuthorId = 1
                },
                new Book()
                {
                    Id = 2,
                    Name = "Dummy",
                    PageCount = 100,
                    AuthorId = 1
                },
                new Book()
                {
                    Id = 3,
                    Name = "Pirates of Carraiben",
                    PageCount = 430,
                    AuthorId = 2
                },
                new Book()
                {
                    Id = 4,
                    Name = "Mars",
                    PageCount = 250,
                    AuthorId = 2
                },
                new Book()
                {
                    Id = 5,
                    Name = "Life",
                    PageCount = 50,
                    AuthorId = 3
                },
                new Book()
                {
                    Id = 6,
                    Name = "Cycling",
                    PageCount = 50,
                    AuthorId = 3
                },
                new Book()
                {
                    Id = 7,
                    Name = "Boys",
                    PageCount = 340,
                    AuthorId = 4
                },
                new Book()
                {
                    Id = 8,
                    Name = "Boysd",
                    PageCount = 340,
                    AuthorId = 1
                },
                new Book()
                {
                    Id = 9,
                    Name = "Boyss",
                    PageCount = 340,
                    AuthorId = 1
                },
                new Book()
                {
                    Id = 10,
                    Name = "Boysz",
                    PageCount = 340,
                    AuthorId = 3
                },
                new Book()
                {
                    Id = 11,
                    Name = "Boysc",
                    PageCount = 340,
                    AuthorId = 2
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
