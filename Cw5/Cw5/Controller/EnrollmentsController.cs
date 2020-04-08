using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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


            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "select * from student inner join Enrollment on Student.IdEnrollment=Enrollment.IdEnrollment where indexnumber=@index";

                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    st.Semester = int.Parse(dr["Semester"].ToString());
                    st.IdStudy = int.Parse(dr["IdStudy"].ToString());
                    st.StartDate = dr["StartDate"].ToString();
                    return Ok(st);
                }

            }





            return Ok(resp);
        }
    }
}