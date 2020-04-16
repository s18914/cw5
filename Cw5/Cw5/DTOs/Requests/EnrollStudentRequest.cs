using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Requests
{
    public class EnrollStudentRequest : Attribute
    {
        [Required(ErrorMessage = "podaj index")]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "podaj imię")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "podaj nazwisko")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "podaj date urodzenia")]
        public string BirthDay { get; set; }

        [Required(ErrorMessage = "podaj kierunek studiów")]
        public string Studies { get; set; }
    }
}

