using DatabaseProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Windows;


namespace DatabaseProject.Controllers
{
    
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Student_Main_Page()
        {
            return View();
        }
        public ActionResult Student_DashBoard()
        {
            return View();
        }

        public ActionResult Student_Register()
        {
            return View();
        }

        public ActionResult Student_Login()
        {
            return View();
        }

        public ActionResult Phone_numbers()
        {
            return View();
        }

        public ActionResult CourseRequest()
        {
            return View();
        }

        public ActionResult CHRequest()
        {
            return View();
        }
        public ActionResult Optional()
        {
            return View();
        }
       
        public ActionResult Required()
        {
            return View();
        }

        public ActionResult Missing()
        {
            return View();
        }
        public ActionResult Available()
        {
            return View();
        }

        public ActionResult Main_Page()
        {
            return View();
        }





        public ActionResult registerStudent(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_StudentRegistration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["f_name"]) || String.IsNullOrWhiteSpace(form["l_name"]) || String.IsNullOrWhiteSpace(form["password"]) || String.IsNullOrWhiteSpace(form["faculty"]) || String.IsNullOrWhiteSpace(form["email"]) || String.IsNullOrWhiteSpace(form["major"]) || String.IsNullOrWhiteSpace(form["semester"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Student_Register");
                    }
                    else
                    {
                       
                            cmd.Parameters.AddWithValue("@first_name", form["f_name"]);
                            cmd.Parameters.AddWithValue("@last_name", form["l_name"]);
                            cmd.Parameters.AddWithValue("@password", form["password"]);
                            cmd.Parameters.AddWithValue("@faculty", form["faculty"]);
                            cmd.Parameters.AddWithValue("@email", form["email"]);
                            cmd.Parameters.AddWithValue("@major", form["major"]);
                            cmd.Parameters.AddWithValue("@Semester", form["semester"]);
                            cmd.Parameters.Add("@Student_id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();  
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data entered wrong.Try again");
                            return RedirectToAction("Student_Register");
                        }
                        int login = Convert.ToInt32(cmd.Parameters["@Student_id"].Value);
                        con.Close();
                        MessageBox.Show("Register successful your ID is " + login);

                        return RedirectToAction("Student_Main_Page");





                    }
                }
            }
        }

        public ActionResult loginStudent(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT dbo.FN_StudentLogin(@Student_id,@password) AS Access", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {

                    Response.Cookies.Clear();
                    if (String.IsNullOrWhiteSpace(form["student_id"]) || String.IsNullOrWhiteSpace(form["password"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Student_Login");
                    }
                 

                    else
                    {          
                         
                        


                        cmd.Parameters.AddWithValue("@Student_id", form["student_id"]);
                        cmd.Parameters.AddWithValue("@password", form["password"]);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data entered wrong.Try again");
                            return RedirectToAction("Student_Login");
                        }

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                                while (rdr.Read())
                                {
                                    
                                    bool login = Convert.ToBoolean(rdr["Access"]);

                                    if (login)
                                    {
                                        
                                        HttpCookie userData = new HttpCookie("userData");
                                        userData["id"] = form["student_id"];
                                        userData["type"] = "Student";
                                        Response.Cookies.Add(userData);
                                       


                                        MessageBox.Show("Login successful");
                                        con.Close();
                                        return RedirectToAction("Student_Dashboard");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Login details are wrong.Try again");
                                        con.Close();
                                        return RedirectToAction("Student_Login");
                                    }
                                }

                        }
                            con.Close();
                            MessageBox.Show("Login successful");
                            return RedirectToAction("Student_Main_Page");

                        
                        
                    }
                }
            }
        }


        public ActionResult addphone(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                    SqlCommand cmd = new SqlCommand("dbo.Procedures_StudentaddMobile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (cmd)
                    {

                        
                        

                        if (String.IsNullOrWhiteSpace(form["phone_number"]))
                        {
                            MessageBox.Show("One of inputs is empty.Try again");
                            return RedirectToAction("Phone_numbers");
                        }
                        else
                        {
                                HttpCookie userData = Request.Cookies["userData"];
                                cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(userData["id"]));
                                cmd.Parameters.AddWithValue("@mobile_number", form["phone_number"]);

                                try 
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                   
                                }
                                catch(Exception)
                                {
                                    con.Close();
                                    
                                    MessageBox.Show("Data inserted wrong.Try again"+ Convert.ToInt32(userData["id"]));
                                    return RedirectToAction("Phone_numbers");
                                }
                                con.Close();
                                MessageBox.Show("Addphone successful");
                                return RedirectToAction("Student_Dashboard");

                        }
                    }
            }
        }

        public ActionResult courserequestFunction(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_StudentSendingCourseRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["course_id"]) || String.IsNullOrWhiteSpace(form["type"]) || String.IsNullOrWhiteSpace(form["comment"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("CourseRequest");
                    }
                    else
                    {
                        HttpCookie userData = Request.Cookies["userData"];

                        cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(userData["id"]));
                        cmd.Parameters.AddWithValue("@courseID", form["course_id"]);
                        cmd.Parameters.AddWithValue("@type", form["type"]);
                        cmd.Parameters.AddWithValue("@comment", form["comment"]);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            
                            
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data inserted wrong.Try again");
                            return RedirectToAction("CourseRequest");
                        }
                        con.Close();
                        MessageBox.Show("Request added successful");
                        return RedirectToAction("Student_Dashboard");

                    }
                }
            }
        }

        public ActionResult CHrequestFunction(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_StudentSendingCHRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["credit_hours"])|| String.IsNullOrWhiteSpace(form["type"]) || String.IsNullOrWhiteSpace(form["comment"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("CHRequest");
                    }
                    else
                    {
                        HttpCookie userData = Request.Cookies["userData"];

                        cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(userData["id"]));
                        cmd.Parameters.AddWithValue("@credit_hours", form["credit_hours"]);
                        
                        cmd.Parameters.AddWithValue("@type", form["type"]);
                        cmd.Parameters.AddWithValue("@comment", form["comment"]);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data is wrong.Try again");
                            return RedirectToAction("CHRequest");
                        }
                        con.Close();
                        MessageBox.Show("Request added successful");
                        return RedirectToAction("Student_Dashboard");     

                    }
                }
            }
        }


        public ActionResult optionalCourses(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_ViewOptionalCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["semester_code"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Optional");
                    }
                    else
                    {

                        List<Models.Course> courses = new List<Models.Course>();
                        HttpCookie userData = Request.Cookies["userData"];

                        cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(userData["id"]));
                        cmd.Parameters.AddWithValue("@current_semester_code", form["semester_code"]);


                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data is wrong.Try again");
                            return RedirectToAction("Optional");
                        }





                            
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {

                                while (rdr.Read())
                                {
                                    Course c1 = new Course(Convert.ToInt16(rdr["course_id"]), (rdr["name"]).ToString());
                                    courses.Add(c1);

                                }

                            }

                        con.Close();
                        MessageBox.Show("Successful");
                        return View(courses);
                    }
                   

                }
                
            }
        }
            public ActionResult requiredCourses(FormCollection form)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("dbo.Procedures_ViewRequiredCourses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (cmd)
                    {
                    if (String.IsNullOrWhiteSpace(form["semester_code"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Required");
                    }

                    else
                    {
                        List<Models.Course> courses = new List<Models.Course>();
                        HttpCookie userData = Request.Cookies["userData"];

                        cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(userData["id"]));
                        cmd.Parameters.AddWithValue("@current_semester_code", form["semester_code"]);


                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data is wrong.Try again");
                            return RedirectToAction("Required");
                        }


                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Course c1 = new Course(Convert.ToInt16(rdr["course_id"]), (rdr["name"]).ToString());
                                courses.Add(c1);

                            }

                        }
                        con.Close();
                        MessageBox.Show("Successful");
                        return View(courses);
                    }
                    }
                }
            }

        public ActionResult missingCourses(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("Procedures_ViewMS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {




                    List<Models.Course> courses = new List<Models.Course>();
                    HttpCookie userData = Request.Cookies["userData"];

                    cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(userData["id"]));

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception)
                    {
                        con.Close();
                        MessageBox.Show("Data is wrong.Try again");
                        return RedirectToAction("Student_Dashboard");
                    }
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Course c1 = new Course(Convert.ToInt16(rdr["course_id"]), (rdr["name"]).ToString());
                            courses.Add(c1);

                        }

                    }
                    con.Close();
                    MessageBox.Show("Successful");
                    return View(courses);
                }
            }
        }
        public ActionResult availableCourses(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.FN_SemsterAvailableCourses(@semstercode)", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["semester_code"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Available");
                    }
                    else
                    {
                        List<Models.Course> courses = new List<Models.Course>();
                        cmd.Parameters.AddWithValue("@semstercode", form["semester_code"]);


                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data is wrong.Try again");
                            return RedirectToAction("Available");
                        }


                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Course c1 = new Course(Convert.ToInt16(rdr["course_id"]), (rdr["name"]).ToString());
                                courses.Add(c1);

                            }

                        }
                        con.Close();
                        MessageBox.Show("Successful");
                        return View(courses);
                    }
                }
            }
        }


    }
}
    