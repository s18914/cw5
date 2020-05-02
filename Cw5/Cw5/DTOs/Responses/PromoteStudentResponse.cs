using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW5.DTOs.Response
{
    public class PromoteStudentResponse
    {
        public int IdEnrollment { get; set; }
        public string Semester { get; set; }
        public string IdStudy { get; set; }
        public DateTime StartDate { get; set; }
    }
}
