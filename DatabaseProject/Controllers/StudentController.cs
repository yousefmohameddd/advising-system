
using DatabaseProject.Models;
using Glimpse.Core.ClientScript;
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
        public ActionResult missingCourses()
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


        //BOLLA Student Part2

        public ActionResult First_MakeupView()
        {
            return View();
        }
        public ActionResult ChooseInstructorView()
        {
            return View();
        }
        public ActionResult InstallmentView()
        {
            return View();
        }
        public ActionResult Second_MakeupView()
        {
            return View();
        }
        public ActionResult SlotsView()
        {
            return View();
        }
      
        public ActionResult RegisterFirstMakeup(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_StudentRegisterFirstMakeup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["course.course_id"]) || String.IsNullOrWhiteSpace(form["semester_code"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("First_MakeupView");
                    }
                    cmd.Parameters.AddWithValue("@StudentID", Request.Cookies["userData"]["id"]);
                    cmd.Parameters.AddWithValue("@CourseID", form["course.course_id"]);
                    cmd.Parameters.AddWithValue("@studentCurr_sem", form["semester_code"]);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception)
                    {
                        con.Close();
                        MessageBox.Show("Data error.Try again");
                        return RedirectToAction("First_MakeupView");

                    }
                    con.Close();
                    MessageBox.Show("Register successful");
                    return RedirectToAction("Student_Dashboard");


                }

            }

        }
        public ActionResult ChooseInstructor(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_Chooseinstructor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(Request.Cookies["userData"]["id"]) || String.IsNullOrWhiteSpace(form["course.course_id"]) || String.IsNullOrWhiteSpace(form["semester_code"]) || String.IsNullOrWhiteSpace(form["instructor.instructor_id"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("CHRequest");
                    }
                    cmd.Parameters.AddWithValue("@StudentID", Request.Cookies["userData"]["id"]);
                    cmd.Parameters.AddWithValue("@instrucorID", form["instructor.instructor_id"]);
                    cmd.Parameters.AddWithValue("@CourseID", form["course.course_id"]);
                    cmd.Parameters.AddWithValue("@current_semester_code", form["semester_code"]);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        con.Close();
                        MessageBox.Show("Data error.Try again");
                        return RedirectToAction("ChooseInstructorView");
                    }


                }
                con.Close();
                MessageBox.Show("Successful");
                return RedirectToAction("Student_Dashboard");
            }

        }
        public ActionResult InstallmentDeadline()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT dbo.FN_StudentUpcoming_installment(@student_ID) AS deadline", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(Request.Cookies["userData"]["id"]))
                    {
                        MessageBox.Show("One of the inputs is empty.Try again ");
                        return RedirectToAction("InstallmentView");
                    }
                    cmd.Parameters.AddWithValue("@student_ID", Request.Cookies["userData"]["id"]);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        con.Close();
                        MessageBox.Show("One of the inputs is empty.Try again ");
                        return RedirectToAction("InstallmentView");
                    }

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            String date = rdr["deadline"].ToString();
                            MessageBox.Show("Nearest deadline is " + date);
                        }

                    }
                    con.Close();
                    return RedirectToAction("Student_Dashboard");

                }
            }
        }
        public ActionResult RegisterSecondMakeup(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_StudentRegisterSecondMakeup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(Request.Cookies["userData"]["id"]) || String.IsNullOrWhiteSpace(form["course.course_id"]) || String.IsNullOrWhiteSpace(form["semester_code"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Second_MakeupView");
                    }
                    cmd.Parameters.AddWithValue("@StudentID", Request.Cookies["userData"]["id"]);
                    cmd.Parameters.AddWithValue("@CourseID", form["Course.course_id"]);
                    cmd.Parameters.AddWithValue("@studentCurr_sem", form["semester_code"]);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        con.Close();
                        MessageBox.Show("Data error.Try again");
                        return RedirectToAction("Second_MakeupView");
                    }


                }

                con.Close();
                MessageBox.Show("Register successful");
                return RedirectToAction("Student_Dashboard");
            }
        }
        public ActionResult SlotsCourseInstructor(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.FN_StudentViewSlot(@CourseID,@InstructorID)", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    if (String.IsNullOrWhiteSpace(form["course.course_id"]) || String.IsNullOrWhiteSpace(form["instructor.instructor_id"]))
                    {
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("SlotsView");
                    }

                    {
                        List<Slot> slots = new List<Slot>();
                        cmd.Parameters.AddWithValue("@CourseID", form["course.course_id"]);
                        cmd.Parameters.AddWithValue("@InstructorID", form["instructor.instructor_id"]);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data error.Try again");
                            return RedirectToAction("SlotsView");
                        }



                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Slot s1 = new Slot(Convert.ToInt16(rdr["slot_id"]), (rdr["day"]).ToString(), (rdr["time"]).ToString(), (rdr["location"]).ToString(), (rdr["Course"]).ToString(), (rdr["Instructor"]).ToString());
                                slots.Add(s1);

                            }

                        }
                        con.Close();
                        MessageBox.Show("Successful");
                        return View(slots);
                    }
                }
            }
        }
        public ActionResult CourseWithSlotDetails()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Courses_Slots_Instructor ", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    {
                        List<Slot> slots = new List<Slot>();
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data error.Try again");
                            return RedirectToAction("First_MakeupView");
                        }



                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Slot s2 = new Slot(Convert.ToInt16(rdr["course_id"]), (rdr["Course"]).ToString(), (Convert.ToInt16(rdr["slot_id"])), (rdr["day"]).ToString(), (rdr["time"]).ToString(), (rdr["location"]).ToString(), (rdr["instructor"]).ToString());
                                slots.Add(s2);

                            }

                        }
                        con.Close();
                        MessageBox.Show("Successful");
                        return View(slots);
                    }
                }
            }
        }
        public ActionResult CourseWithExamDetails()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Courses_MakeupExams", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {


                    {
                        List<MakeUp_Exam> makeUp_Exams = new List<MakeUp_Exam>();
                      
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("Data error.Try again");
                            return RedirectToAction("Student_Dashboard");
                        }



                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                MakeUp_Exam M1 = new MakeUp_Exam((rdr["name"]).ToString(), Convert.ToInt16(rdr["semester"]), Convert.ToInt16(rdr["exam_id"]), (rdr["date"]).ToString(), (rdr["type"]).ToString());
                                makeUp_Exams.Add(M1);

                            }

                        }
                        con.Close();
                        MessageBox.Show("Successful");
                        return View(makeUp_Exams);
                    }
                }
            }
        }
        public ActionResult CoursesWithPrerequisites()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.view_Course_prerequisites", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {


                    {
                        List<PrerequisitesCourses> prerequisites = new List<PrerequisitesCourses>();
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            con.Close();
                            MessageBox.Show("One of inputs is empty.Try again");
                            return RedirectToAction("Student_Dashboard");
                        }



                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                PrerequisitesCourses P1 = new PrerequisitesCourses(Convert.ToInt16(rdr["course_id"]), Convert.ToInt16(rdr["preRequsite_course_id"]));
                                prerequisites.Add(P1);

                            }

                        }
                        con.Close();
                        MessageBox.Show("Successful"); 
                        return View(prerequisites);
                    }
                }
            }
        }
        public ActionResult GradPlanView()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.FN_StudentViewGP(@student_id)", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    List<GradPlanCourse> GradPlanCourse = new List<GradPlanCourse>();
                    cmd.Parameters.AddWithValue("@student_id", Request.Cookies["userData"]["id"]);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        con.Close();
                        MessageBox.Show("One of inputs is empty.Try again");
                        return RedirectToAction("Student_Dashboard");
                    }


                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            GradPlanCourse g = new GradPlanCourse();
                            g.gradPlan.plan_id = Convert.ToInt16(rdr["plan_id"]);
                            GradPlanCourse G1 = new GradPlanCourse(Convert.ToInt16(rdr["plan_id"]), (rdr["semester_code"]).ToString(), Convert.ToInt16(rdr["course_id"]));
                            GradPlanCourse.Add(G1);

                        }

                    }
                    con.Close();
                    MessageBox.Show("Successful");
                    return View(GradPlanCourse);
                }
            }
        }


    }
}
    