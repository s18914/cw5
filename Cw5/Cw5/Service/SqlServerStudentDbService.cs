
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
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
        public void EnrollStudent(EnrollStudentRequest req)
        {
            var newSt = new Student();
            newSt.FirstName = req.FirstName;
            newSt.LastName = req.LastName;
            newSt.IndexNumber = req.IndexNumber;
            newSt.BirthDate = req.BirthDate;
            newSt.Studies = req.Studies;

            var resp = new EnrollStudentResponse();
            resp.LastName = newSt.LastName;
           String ConString = "Data Source=db-mssql;Initial Catalog=s18914;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
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
                        tran.Rollback();
                        //return BadRequest("Nie ma takich studiow");
                    }
                    dr.Close();
                    int idStudies = (int)dr["IdStudies"];

                    //dodam studenta
                    int id_enrollment = 0;
                    com.CommandText = "SELECT MAX(IDENROLLMENT) AS IdEnrollment FROM ENROLLMENT WHERE (ENROLLMENT.IdStudy = (SELECT STUDIES.IdStudy FROM STUDIES WHERE Studies.NAME = @name)) AND ENROLLMENT.Semester = 1;";
                    com.Parameters.AddWithValue("name", req.Studies);
                    ////

                    dr = com.ExecuteReader();

                    if (!dr.Read()) //nie istnieje taki wpis
                    {
                        dr.Close();

                        com.CommandText = "SELECT IDSTUDY FROM STUDIES WHERE NAME = @name;";
                        com.Parameters.AddWithValue("@name", req.Studies);
                        dr = com.ExecuteReader();
                        int newIdS = (int)(dr["IDSTUDY"]);
                        dr.Close();

                        com.CommandText = "SELECT MAX(IDENROLLMENT) AS newIdE FROM ENROLLMENT;";
                        dr = com.ExecuteReader();
                        int newIdE = (int)(dr["newIdE"]);
                        int idE = newIdE + 1;
                        dr.Close();

                        com.CommandText = "INSERT INTO ENROLLMENT VALUES (@IdE, 1, @IdS, @date);";
                        com.Parameters.AddWithValue("IdE", idE);
                        com.Parameters.AddWithValue("IdS", newIdS);
                        com.Parameters.AddWithValue("date", DateTime.Now);
                        com.ExecuteNonQuery();
                        dr.Close();

                        id_enrollment = newIdS;

                    }

                    else
                    {
                        id_enrollment = (int)(dr["IdEnrollment"]);
                        dr.Close();
                    }


                    com.CommandText = com.CommandText = "INSERT INTO  STUDENT VALUES (@Index, @Fname, @Lname, @Birth, @idEnroll)";
                    com.Parameters.AddWithValue("Index", req.IndexNumber);
                    com.Parameters.AddWithValue("Fname", req.FirstName);
                    com.Parameters.AddWithValue("Lname", req.LastName);
                    com.Parameters.AddWithValue("Birth", Convert.ToDateTime(req.BirthDate));
                    com.Parameters.AddWithValue("idEnroll", id_enrollment);


                    tran.Commit();

                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }


            }
        }
        public void PromoteStudents(int semester, string studies)
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
    }
}
