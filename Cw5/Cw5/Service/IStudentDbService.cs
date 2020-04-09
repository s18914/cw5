using System.Collections;
using Cw5.DTOs.Requests;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;

namespace CW5.Services
{
    public interface IStudentsDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(int semester, string studies);
    }
}
