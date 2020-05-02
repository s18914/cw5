
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using CW5.DTOs.Request;
using CW5.DTOs.Response;
using CW5.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Service
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest req)
        {
           

            var enrollRepso = new EnrollStudentResponse();
            //resp.LastName = newSt.LastName;
           String ConString = "Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            using (var tran = con.BeginTransaction())
            {
                com.Connection = con;
                con.Open();
                try
                {

                    //Czy istnieją takie studia
                    com.CommandText = "select IdStudies from studies where name=@name";
                    //przekazuje parametr
                    com.Parameters.AddWithValue("name", req.Studies);

                    //uruchamiam
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        //return BadRequest("Nie ma takich studiow");
                    }
                    dr.Close();
                    int idStudies = (int)dr["IdStudies"];

                    //dodam studenta
                    int idEnrollment = 0;
                    com.CommandText = "SELECT MAX(IDENROLLMENT) AS IdEnrollment FROM ENROLLMENT WHERE (ENROLLMENT.IdStudy = (SELECT STUDIES.IdStudy FROM STUDIES WHERE Studies.NAME = @name)) AND ENROLLMENT.Semester = 1;";
                    //com.Parameters.AddWithValue("name", req.Studies);
                    ////

                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        idEnrollment = 1;
                        dr.Close();
                    }
                    else
                    {

                        com.CommandText = "select Max(IdEnrollment) As maxEnroll from Enrollment";
                        idEnrollment = (int)dr["maxEnroll"];
                        com.CommandText = "insert into enrollment (IdEnrollment,semester,Idstudy,startdate)" + "values(IdEnrollment,1,@idstudy,@date)";

                    }
                    dr.Close();

                    com.Parameters.AddWithValue("IdStudies", req.Studies);
                    com.Parameters.AddWithValue("date", DateTime.Now.ToString());

                    com.ExecuteNonQuery();


                    com.CommandText = "Insert into Student(IndexNumber, Firstname, lastname, birthday, studies,semester,IdEnrollment) values(@Index,@fname,@lname,@bday,@stud,@IdEnrollment)";
                    com.Parameters.AddWithValue("index", req.IndexNumber);
                    com.Parameters.AddWithValue("fname", req.FirstName);
                    com.Parameters.AddWithValue("lname", req.LastName);
                    com.Parameters.AddWithValue("bday", req.BirthDay);
                    com.Parameters.AddWithValue("stud", req.Studies);
                    com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                    com.ExecuteNonQuery();





                    enrollRepso.IdEnrollment = idEnrollment;
                    enrollRepso.Semester = 1;
                    enrollRepso.Name = dr["name"].ToString();
                    enrollRepso.StartDate = DateTime.Parse(dr["date"].ToString());



                    tran.Commit();

                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }

                return enrollRepso;
            }
        }

        public PromoteStudentResponse PromoteStudent(PromoteStudentRequest psrequest)
        {

            using (SqlConnection con = new SqlConnection("Data Source = db - mssql; Initial Catalog = s18914; Integrated Security = True"))
            using (SqlCommand com = new SqlCommand())
            using (var tran = con.BeginTransaction())
            {
                PromoteStudentResponse response = new PromoteStudentResponse();
                con.Open();
                com.Connection = con;
                com.Transaction = tran;
                try
                {


                    com.CommandText = "exec PromoteStudents @name, @semester";
                    com.Parameters.AddWithValue("semester", psrequest.Semester);
                    com.Parameters.AddWithValue("name", psrequest.Name);
                    com.ExecuteNonQuery();


                    com.CommandText = "SELECT * FROM Enrollment e INNER JOIN Studies stud ON stud.idstudy = e.idstudy WHERE e.semester = Semest AND stud.name = stName";
                    com.Parameters.AddWithValue("stName", psrequest.Name);
                    com.Parameters.AddWithValue("Semest", psrequest.Semester);
                    var dr = com.ExecuteReader();


                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                    }
                    else
                    {
                        response.IdEnrollment = (int)dr["IdEnrollment"];
                        response.IdStudy = dr["IdStudy"].ToString();
                        response.Semester = dr["Semester"].ToString();
                        response.StartDate = (DateTime)dr["StartDate"];

                    }


                }
                catch (SqlException e)
                {
                    tran.Rollback();

                }

                return response;
            }


        }

        /*
         * public void PromoteStudents(int semester, string studies)
        {
            String ConString = "Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.Parameters.AddWithValue("@studies", studies);
                com.Parameters.AddWithValue("@semester", semester);
                SqlDataReader dataReader = com.ExecuteReader();

                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return;
                }

                com.CommandText = "PromoteStudents";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                //com.Parameters.AddWithValue(semester, studies);

            }
        }
         * 
         */
        

        public bool StudentExist(string indexNumber)
        {
            bool isExist = false;

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True"))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "SELECT indexNumber FROM Student WHERE indexNumber = @indexNumber; ";
                com.Parameters.AddWithValue("indexNumber", indexNumber);

                con.Open();
                var dr = com.ExecuteReader();

                if (dr.Read())
                {
                    isExist = true;
                }

            }

            return isExist;
        }


        //EnrollStudentResponse IStudentsDbService.EnrollStudent(EnrollStudentRequest erequest)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
