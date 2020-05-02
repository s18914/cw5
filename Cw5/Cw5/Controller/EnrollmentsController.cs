using System;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using CW5.DTOs.Request;
using CW5.DTOs.Response;
using CW5.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controller
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private const String ConString = "Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True";

        private readonly IStudentsDbService _dbService;

        public EnrollmentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;
        }


        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {

            _dbService.EnrollStudent(req);
            var response = new EnrollStudentResponse();

            return CreatedAtAction("EnrollStudent", response);
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentRequest preq)
        {

            PromoteStudentResponse pres = new PromoteStudentResponse();

            pres = _dbService.PromoteStudent(preq);
            return Created("promote", pres);
        }


    }
}