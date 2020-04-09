using Cw5.DAL;
using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CW5.DAL
{

    public class MockDbService : IDbService
    {
        private static List<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student { FirstName = "Jan", LastName = "Kowalski"},
                new Student { FirstName = "Anna", LastName = "Malewski"},
                new Student { FirstName = "Andrzej", LastName = "Andrzejewski"}
            };

        }


        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        
    }
}
