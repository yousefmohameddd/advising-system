using DatabaseProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;



namespace AdvisorProject.Controllers
{
    public class AdvisorController : Controller
    {
        // GET: Advisor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ViewStudents()
        {
            return advisorViewStudents();
        }

        public ActionResult CreateGraduationPlan()
        {
            ViewData["semesterData"] = new SelectList(getAllSemesters(), "semester_code", "semester_code");
            ViewData["studentData"] = new SelectList(getAllStudents(), "student_id", "student_id");
            return View();
        }
        public ActionResult GetMajor()
        {
            ViewData["majorData"] = new SelectList(getAllMajors(), "major", "major");
            return View();
        }
        public ActionResult InsertCourseIntoGraduationPlan()
        {
            ViewData["semesterData"] = new SelectList(getAllSemesters(), "semester_code", "semester_code");
            ViewData["studentData"] = new SelectList(getAllStudents(), "student_id", "student_id");
            ViewData["courseData"] = new SelectList(getAllCourseNames(), "name", "name");
            return View();
        }

        public ActionResult UpdateGraduationDate()
        {
            ViewData["studentData"] = new SelectList(getAllStudents(), "student_id", "student_id");
            return View();
        }

        public ActionResult DeleteGraduationPlanCourse()
        {
            ViewData["semesterData"] = new SelectList(getAllSemesters(), "semester_code", "semester_code");
            ViewData["studentData"] = new SelectList(getAllStudents(), "student_id", "student_id");
            ViewData["courseData"] = new SelectList(getAllCourseIDs(), "course_id", "course_id");
            return View();
        }

        public ActionResult ViewRequests()
        {
            return advisorViewRequests();
        }

        public ActionResult ViewPendingRequests()
        {
            return advisorViewPendingRequests();
        }

        public ActionResult AcceptCreditRequest()
        {
            ViewData["requestData"] = new SelectList(getAllRequests(), "request_id", "request_id");
            ViewData["semesterData"] = new SelectList(getAllSemesters(), "semester_code", "semester_code");
            return View();
        }

        public ActionResult AcceptCourseRequest()
        {
            ViewData["semesterData"] = new SelectList(getAllSemesters(), "semester_code", "semester_code");
            ViewData["requestData"] = new SelectList(getAllRequests(), "request_id", "request_id");
            return View();
        }
        public ActionResult Return()
        {
            HttpCookie authCookie = new HttpCookie("advisorData");
            authCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(authCookie);
            return RedirectToAction("./Main_Page");
        }

        public ActionResult Logout()
        {
            HttpCookie authCookie = new HttpCookie("advisorData");
            authCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(authCookie);
            return RedirectToAction("./Login");
        }

        private List<Student> getAllMajors()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT major FROM Student", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Student> majors = new List<Student>();
                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(rdr.GetOrdinal("major")))
                            {
                                String major = Convert.ToString(rdr["major"]);
                                majors.Add(new Student(major));
                            }
                        }
                        con.Close();
                        return majors;
                    }
                }
            }

        }

        private List<Semester> getAllSemesters()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT semester_code FROM semester", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Semester> semesters = new List<Semester>();
                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(rdr.GetOrdinal("semester_code")))
                            {
                                String semester = Convert.ToString(rdr["semester_code"]);
                                semesters.Add(new Semester(semester));
                            }
                        }
                        con.Close();
                        return semesters;
                    }
                }
            }
        }

        private List<Student> getAllStudents()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.AdminListStudentsWithAdvisors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    List<Student> students = new List<Student>();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read() && rdr.HasRows)
                        {

                            int studentId = Convert.ToInt32(rdr["student_id"]);
                            int advisorId = Convert.ToInt32(rdr["advisor_id"]);
                            HttpCookie advisorData = Request.Cookies["advisorData"];

                            if (advisorId == Convert.ToInt32(advisorData["id"]))
                            {
                                students.Add(new Student(studentId));
                            }

                        }
                        con.Close();
                        return students;
                    }
                }
            }
        }

        private List<Request> getAllRequests()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorViewPendingRequests", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    HttpCookie advisorData = Request.Cookies["advisorData"];
                    cmd.Parameters.AddWithValue("@Advisor_ID", Convert.ToInt32(advisorData["id"]));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Request> requests = new List<Request>();
                        while (rdr.Read() && rdr.HasRows)
                        {

                            Request request = new Request(Convert.ToInt32(rdr["request_id"]));
                            requests.Add(request);
                        }
                        con.Close();
                        return requests;
                    }
                }
            }
        }

        private List<Course> getAllCourseNames()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT name FROM Course", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Course> courses = new List<Course>();
                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(rdr.GetOrdinal("name")))
                            {
                                String name = Convert.ToString(rdr["name"]);
                                courses.Add(new Course(name));
                            }
                        }
                        con.Close();
                        return courses;
                    }
                }
            }
        }

        private List<Course> getAllCourseIDs()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT course_id FROM Course", con);
            cmd.CommandType = CommandType.Text;
            using (cmd)
            {
                con.Open();
                cmd.ExecuteNonQuery();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    List<Course> courses = new List<Course>();
                    while (rdr.Read())
                    {

                        if (!rdr.IsDBNull(rdr.GetOrdinal("course_id")))
                        {
                            int id = Convert.ToInt32(rdr["course_id"]);
                            courses.Add(new Course(id));
                        }
                    }
                    con.Close();
                    return courses;
                }
            }
        }

        //A Register Adivosr
        public ActionResult RegisterAdvisor(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorRegistration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrEmpty(form["advisor_name"]) || String.IsNullOrEmpty(form["password"]) || String.IsNullOrEmpty(form["email"]) || String.IsNullOrEmpty(form["office"]))
                    {
                        TempData["ErrorMessage"] = "Please fill out all the boxes";
                        return RedirectToAction("./Register");
                    }
                    cmd.Parameters.AddWithValue("@advisor_name", form["advisor_name"]);
                    cmd.Parameters.AddWithValue("@password", form["password"]);
                    cmd.Parameters.AddWithValue("@email", form["email"]);
                    cmd.Parameters.AddWithValue("@office", form["office"]);
                    SqlParameter advisorID = cmd.Parameters.AddWithValue("@Advisor_id", SqlDbType.Int);
                    advisorID.Direction = ParameterDirection.Output;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    TempData["SuccessMessage"] = "You have succesfully registered, with ID: " + Convert.ToInt16(advisorID.Value) + ".  Please login" +
                        "using that ID number.";
                    return RedirectToAction("./Login");
                }

            }

        }

        //B Login Advisor
        public ActionResult LoginAdvisor(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT dbo.FN_AdvisorLogin(@advisor_Id,@password) AS Success", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@advisor_Id", form["advisor_id"]);
                    cmd.Parameters.AddWithValue("@password", form["password"]);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {

                            bool login = Convert.ToBoolean(rdr["Success"]);

                            if (login)
                            {
                                HttpCookie userData = new HttpCookie("advisorData");
                                userData["id"] = form["advisor_id"];
                                Response.Cookies.Add(userData);
                                con.Close();
                                return RedirectToAction("./Index");
                            }
                            else
                            {
                                con.Close();
                                TempData["ErrorMessage"] = "No login exists for that ID/password";
                                return RedirectToAction("./Login");
                            }
                        }

                    }
                    con.Close();
                    return RedirectToAction("./Login");
                }
            }
        }

        //C View all students for this advisor
        public ActionResult advisorViewStudents()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                HttpCookie advisorData = Request.Cookies["advisorData"];
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE advisor_id=" + Convert.ToInt32(advisorData["id"]), con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    List<Student> students = new List<Student>();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read() && rdr.HasRows)
                        {

                            int studentId = Convert.ToInt32(rdr["student_id"]);
                            String studentFirstName = Convert.ToString(rdr["f_name"]);
                            String studentLastName = Convert.ToString(rdr["l_name"]);
                            String studentFaculty = Convert.ToString(rdr["faculty"]);
                            String studentEmail = Convert.ToString(rdr["email"]);
                            String studentMajor = Convert.ToString(rdr["major"]);
                            bool studentStatus = true;
                            int semester = 0;
                            int acquired_hours = 0;
                            int assigned_hours = 0;
                            decimal studentGPA = 0;

                            if (!rdr.IsDBNull(rdr.GetOrdinal("financial_status")))//et2aked eno mesh null
                                studentStatus = Convert.ToBoolean(rdr["financial_status"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("semester")))//et2aked eno mesh null
                                semester = Convert.ToInt32(rdr["semester"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("gpa")))//et2aked eno mesh null
                                studentGPA = Convert.ToDecimal(rdr["gpa"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("acquired_hours")))//et2aked eno mesh null
                                acquired_hours = Convert.ToInt32(rdr["acquired_hours"]);
                            if (!rdr.IsDBNull(rdr.GetOrdinal("assigned_hours")))//et2aked eno mesh null
                                assigned_hours = Convert.ToInt32(rdr["assigned_hours"]);

                            students.Add(new Student(studentId, studentFirstName, studentLastName, studentGPA, studentFaculty,
                                         studentEmail, studentMajor, studentStatus, semester, acquired_hours, assigned_hours));


                        }
                        con.Close();
                        return View(students);


                    }
                }
            }
        }

        //D Insert grad plan for a certain student
        public ActionResult advisorInsertGradPlan(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorCreateGP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    HttpCookie advisorData = Request.Cookies["advisorData"];
                    int advisorId = Convert.ToInt32(advisorData["id"]);

                    cmd.Parameters.AddWithValue("@Semester_code", form["semester_code"]);
                    cmd.Parameters.AddWithValue("@expected_graduation_date", form["expected_grad_date"]);
                    cmd.Parameters.AddWithValue("@sem_credit_hours", form["semester_credit_hours"]);
                    cmd.Parameters.AddWithValue("@advisor_id", advisorId);
                    cmd.Parameters.AddWithValue("@student_id", form["student.student_id"]);
                    try
                    {
                        con.Open();
                        if (String.IsNullOrEmpty(form["semester_code"]) || String.IsNullOrEmpty(form["expected_grad_date"])
                            || String.IsNullOrEmpty(form["semester_credit_hours"]) || String.IsNullOrEmpty(form["Student.student_id"]))
                        {
                            con.Close();
                            TempData["ErrorMessage"] = "Please fill out all the boxes";
                            return RedirectToAction("./CreateGraduationPlan");
                        }

                        int success = cmd.ExecuteNonQuery();
                        con.Close();
                        if (success <= 0)
                        {
                            TempData["ErrorMessage"] = "Unable to create graduation plan as the student has less than 157 acquired hours" +
                                " or there already exists a graduation plan for this student";
                            return RedirectToAction("./CreateGraduationPlan");
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Graduation Plan Created!";
                            return RedirectToAction("./CreateGraduationPlan");

                        }
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        con.Close();
                        TempData["ErrorMessage"] = "Unable to create graduation plan, please check you have entered a valid date";
                        return RedirectToAction("./CreateGraduationPlan");

                    }



                }
            }
        }

        //E Insert course for a specific graduation plan
        public ActionResult advisorInsertCourseGradPlan(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorAddCourseGP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@student_id", form["gradPlan.student.student_id"]);
                    cmd.Parameters.AddWithValue("@Semester_code", form["semester.semester_code"]);
                    cmd.Parameters.AddWithValue("@course_name", form["course.name"]);

                    try
                    {
                        con.Open();
                        if (String.IsNullOrEmpty(form["gradPlan.student.student_id"]) ||
                            String.IsNullOrEmpty(form["semester.semester_code"]) ||
                            String.IsNullOrEmpty(form["course.name"]))
                        {
                            con.Close();
                            TempData["ErrorMessage"] = "Please fill out all the boxes.";
                            return RedirectToAction("./InsertCourseIntoGraduationPlan");
                        }
                        int success = cmd.ExecuteNonQuery();
                        con.Close();

                        TempData["SuccessMessage"] = "Course has been succesfully inserted!";
                        return RedirectToAction("./InsertCourseIntoGraduationPlan");
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        con.Close();
                        TempData["ErrorMessage"] = "Unable to add course, please check that the student has a graduation plan for that semester and that the course" +
                                                   " is not already in the graduation plan";
                        return RedirectToAction("./InsertCourseIntoGraduationPlan");

                    }

                }
            }
        }

        //F Update graduation date in graduation plan
        public ActionResult advisorUpdateGradDate(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorUpdateGP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@expected_grad_date", form["expected_grad_date"]);
                    cmd.Parameters.AddWithValue("@studentID", form["student.student_id"]);
                    try
                    {
                        con.Open();
                        if (String.IsNullOrEmpty(form["expected_grad_date"]) ||
                            String.IsNullOrEmpty(form["student.student_id"]))
                        {
                            con.Close();
                            TempData["ErrorMessage"] = "Please fill out the boxes";
                            return RedirectToAction("./UpdateGraduationDate");
                        }
                        int success = cmd.ExecuteNonQuery();
                        con.Close();
                        if (success <= 0)
                        {
                            TempData["ErrorMessage"] = "Failed to update graduation plan date because the student does not have a graduation plan";
                            return RedirectToAction("./UpdateGraduationDate");
                        }
                        TempData["SuccessMessage"] = "Graduation date has been successfully updated";
                        return RedirectToAction("./UpdateGraduationDate");
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        con.Close();
                        TempData["ErrorMessage"] = "Failed to update graduation plan date, please check the date inserted was valid";
                        return RedirectToAction("./UpdateGraduationDate");

                    }

                }
            }

        }

        //G Delete course from graduation plan
        public ActionResult advisorDeleteCourse(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.[Procedures_AdvisorDeleteFromGP]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@studentID", form["gradPlan.student.student_id"]);
                    cmd.Parameters.AddWithValue("@sem_code", form["semester.semester_code"]);
                    cmd.Parameters.AddWithValue("@courseID", form["course.course_id"]);

                    try
                    {
                        con.Open();
                        if (String.IsNullOrEmpty(form["gradPlan.student.student_id"]) ||
                            String.IsNullOrEmpty(form["semester.semester_code"]) ||
                            String.IsNullOrEmpty(form["course.course_id"]))
                        {
                            con.Close();
                            TempData["ErrorMessage"] = "Please fill out all the boxes";
                            return RedirectToAction("./DeleteGraduationPlanCourse");
                        }
                        int success = cmd.ExecuteNonQuery();
                        con.Close();
                        if (success <= 0)
                        {
                            TempData["ErrorMessage"] = "Could not find course in graduation plan to delete," +
                                                         " please check that the student has a course in that graduation plan";
                            return RedirectToAction("./DeleteGraduationPlanCourse");

                        }
                        TempData["SuccessMessage"] = "Course has been succesfully deleted";
                        return RedirectToAction("./DeleteGraduationPlanCourse");
                    }
                    catch (System.Data.SqlClient.SqlException)//mafrood te3mel check eno student
                    {
                        con.Close();
                        TempData["ErrorMessage"] = "One of the inputs was written incorrectly";
                        return RedirectToAction("./DeleteGraduationPlanCourse");

                    }
                }
            }
        }
        //H View all students with a certain major
        public ActionResult advisorViewStudentsWithMajor(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorViewAssignedStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (cmd)
                {
                    HttpCookie advisorData = Request.Cookies["advisorData"];
                    cmd.Parameters.AddWithValue("@AdvisorID", Convert.ToInt32(advisorData["id"]));
                    cmd.Parameters.AddWithValue("@major", form["major"]);
                    List<StudentCourseViewModel> courseStudents = new List<StudentCourseViewModel>();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            int studentId = Convert.ToInt32(rdr["student_id"]);
                            String studentName = Convert.ToString(rdr["Student_name"]);
                            String studentMajor = Convert.ToString(rdr["major"]);
                            String courseName = Convert.ToString(rdr["Course_name"]);
                            if (courseName == "")
                                courseName = "No Courses Are Taken";
                            courseStudents.Add(new StudentCourseViewModel(
                                                new Student(studentName, studentId, studentMajor),
                                                new Course(courseName))
                                                );


                        }
                        con.Close();
                        return View("ViewStudentsOnMajorResult", courseStudents);
                    }
                }
            }
        }

        //I view all requests
        public ActionResult advisorViewRequests()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.FN_Advisors_Requests(@advisor_Id)", con);
                cmd.CommandType = CommandType.Text;
                using (cmd)
                {
                    HttpCookie advisorData = Request.Cookies["advisorData"];
                    cmd.Parameters.AddWithValue("@advisor_Id", Convert.ToInt32(advisorData["id"]));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Request> requests = new List<Request>();
                        while (rdr.Read() && rdr.HasRows)
                        {
                            int requestID = Convert.ToInt32(rdr["request_id"]);
                            string type = Convert.ToString(rdr["type"]);
                            string comment = Convert.ToString(rdr["comment"]);
                            string status = Convert.ToString(rdr["status"]);

                            int creditHours = 0;
                            int creditHoursIndex = rdr.GetOrdinal("credit_hours");//geeb el column number bet3a ch
                            if (!rdr.IsDBNull(creditHoursIndex))//et2aked eno mesh null
                            {
                                creditHours = Convert.ToInt32(rdr["credit_hours"]);
                            }

                            int courseID = 0;
                            int courseIDIndex = rdr.GetOrdinal("course_id");//geeb el column number bet3a ch
                            if (!rdr.IsDBNull(courseIDIndex))//et2aked eno mesh null
                            {
                                courseID = Convert.ToInt32(rdr["course_id"]);
                            }

                            int studentID = Convert.ToInt32(rdr["student_id"]);
                            int advisorID = Convert.ToInt32(rdr["advisor_id"]);

                            Student student = new Student(studentID);
                            Advisor advisor = new Advisor(advisorID);
                            Request currentRequest = new Request(requestID, type, comment, status, creditHours,
                                                                 courseID, student, advisor);
                            requests.Add(currentRequest);
                        }
                        con.Close();
                        return View(requests);
                    }
                }
            }

        }

        //J View all pending Requests
        public ActionResult advisorViewPendingRequests()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorViewPendingRequests", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    HttpCookie advisorData = Request.Cookies["advisorData"];
                    cmd.Parameters.AddWithValue("@Advisor_ID", Convert.ToInt32(advisorData["id"]));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        List<Request> requests = new List<Request>();
                        while (rdr.Read() && rdr.HasRows)
                        {
                            int requestID = Convert.ToInt32(rdr["request_id"]);
                            string type = Convert.ToString(rdr["type"]);
                            string comment = Convert.ToString(rdr["comment"]);
                            string status = Convert.ToString(rdr["status"]);

                            int creditHours = 0;
                            int creditHoursIndex = rdr.GetOrdinal("credit_hours");//geeb el column number bet3a ch
                            if (!rdr.IsDBNull(creditHoursIndex))//et2aked eno mesh null
                            {
                                creditHours = Convert.ToInt32(rdr["credit_hours"]);
                            }

                            int courseID = 0;
                            int courseIDIndex = rdr.GetOrdinal("course_id");//geeb el column number bet3a ch
                            if (!rdr.IsDBNull(courseIDIndex))//et2aked eno mesh null
                            {
                                courseID = Convert.ToInt32(rdr["course_id"]);
                            }

                            int studentID = Convert.ToInt32(rdr["student_id"]);
                            int advisorID = Convert.ToInt32(rdr["advisor_id"]);

                            Student student = new Student(studentID);
                            Advisor advisor = new Advisor(advisorID);
                            Request currentRequest = new Request(requestID, type, comment, status, creditHours,
                                                                 courseID, student, advisor);
                            requests.Add(currentRequest);
                        }
                        con.Close();
                        return View(requests);
                    }
                }
            }


        }

        //K Accept/Reject credit hours requests
        public ActionResult advisorEditsCreditRequest(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorApproveRejectCHRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@requestID", form["request.request_id"]);
                    cmd.Parameters.AddWithValue("@current_sem_code", form["semester.semester_code"]);
                    try
                    {
                        con.Open();
                        if (String.IsNullOrEmpty(form["request.request_id"]) ||
                            String.IsNullOrEmpty(form["semester.semester_code"]))
                        {
                            con.Close();
                            TempData["ErrorMessage"] = "One of the boxes was not filled.";
                            return RedirectToAction("./AcceptCreditRequest");
                        }
                        int success = cmd.ExecuteNonQuery();
                        con.Close();
                        if (success <= 0)
                        {
                            TempData["ErrorMessage"] = "An error occurred in the database while update: please try again";
                            return RedirectToAction("./AcceptCreditRequest");
                        }
                        TempData["SuccessMessage"] = getRequestResult(Convert.ToInt32(form["request.request_id"]));
                        return RedirectToAction("./AcceptCreditRequest");
                    }
                    catch (System.Data.SqlClient.SqlException)//mafrood te3mel check eno student
                    {
                        con.Close();
                        TempData["ErrorMessage"] = "One of the inputs was invalid. Please check again.";
                        return RedirectToAction("./AcceptCreditRequest");

                    }
                }
            }
        }


        //L Accept/Reject Course requests
        public ActionResult advisorEditsCourseRequest(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdvisorApproveRejectCourseRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@requestID", form["request.request_id"]);
                    cmd.Parameters.AddWithValue("@current_semester_code", form["semester.semester_code"]);
                    try
                    {
                        con.Open();
                        if (String.IsNullOrEmpty(form["request.request_id"]) ||
                            String.IsNullOrEmpty(form["semester.semester_code"]))
                        {
                            con.Close();
                            TempData["ErrorMessage"] = "One of the boxes was not filled.";
                            return RedirectToAction("./AcceptCourseRequest");
                        }
                        int success = cmd.ExecuteNonQuery();
                        con.Close();
                        if (success <= 0)
                        {
                            TempData["ErrorMessage"] = "Could not find that request ID: It might be already accepted/rejected or no course r" +
                                                       "equest exists with that ID";
                            return RedirectToAction("./AcceptCourseRequest");
                        }
                        TempData["SuccessMessage"] = getRequestResult(Convert.ToInt32(form["request.request_id"]));

                        return RedirectToAction("./AcceptCourseRequest");
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        con.Close();
                        TempData["ErrorMessage"] = "One of the inputs was invalid. Please check again.";
                        return RedirectToAction("./AcceptCourseRequest");

                    }
                }
            }
        }
        private String getRequestResult(int requestID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT status FROM Request WHERE request_id = " + requestID, con);
            cmd.CommandType = CommandType.Text;
            using (cmd)
            {
                con.Open();
                cmd.ExecuteNonQuery();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {

                    while (rdr.Read())
                    {
                        if (Convert.ToString(rdr["status"]) == "Accept")
                        {
                            con.Close();
                            return "Request was accepted!";
                        }
                        else
                        {
                            con.Close();
                            return "Request was rejected!";
                        }
                    }
                    con.Close();
                    return "Request has been updated!";
                }
            }
        }
    }
}



