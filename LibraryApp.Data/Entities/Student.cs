using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Data.Entities
{
    public class Student
    {
        public Student()
        {
            StudentBooks = new List<StudentBook>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<StudentBook> StudentBooks { get; set; }

    }
}
