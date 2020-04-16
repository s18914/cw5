using System.Collections;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using CW5.DTOs.Request;
using CW5.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace CW5.Services
{
    public interface IStudentsDbService
    {
       public EnrollStudentResponse EnrollStudent(EnrollStudentRequest erequest);
        public PromoteStudentResponse PromoteStudent(PromoteStudentRequest prequest);
        public bool StudentExist(string indexNumber);
    }
}
