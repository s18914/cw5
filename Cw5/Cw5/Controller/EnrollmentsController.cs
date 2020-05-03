﻿using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using CW5.DTOs.Request;
using CW5.DTOs.Response;
using CW5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw5.Controller
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private const String ConString = "Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True";

        private readonly IStudentsDbService _dbService;
        private IConfiguration _configuration;


        public EnrollmentsController(IStudentsDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            _configuration = configuration;
        }


        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest req)
        {

            _dbService.EnrollStudent(req);
            var response = new EnrollStudentResponse();

            return CreatedAtAction("EnrollStudent", response);
        }

        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(PromoteStudentRequest preq)
        {

            PromoteStudentResponse pres = new PromoteStudentResponse();

            pres = _dbService.PromoteStudent(preq);
            return Created("promote", pres);
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {

            //sprawdzam login i haslo

            var claims = new[]
           {
                new Claim(ClaimTypes.Name, "Mila123"),
                new Claim(ClaimTypes.Role, "employee")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost",
                audience: "https://localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );


            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = "token"
            });

        }


    }
}