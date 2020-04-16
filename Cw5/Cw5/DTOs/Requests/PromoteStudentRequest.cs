
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CW5.DTOs.Request
{
    public class PromoteStudentRequest
    {
        [Required(ErrorMessage = "Name: ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Semester: ")]
        public int Semester { get; set; }
    }
}
