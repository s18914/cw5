using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controller
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {

            var newSt = new Student();
            newSt.FirstName = req.FirstName;
            newSt.LastName = req.LastName;
            newSt.IndexNumber = req.IndexNumber;
            newSt.BirthDate = req.BirthDate;
            newSt.Studies = req.Studies;

            var resp = new EnrollStudentResponse();
            resp.LastName = newSt.LastName;
            resp.Semester = newSt.Semester;
            resp.StartDate = newSt.StartDate;

            return Ok();
        }
    }
}