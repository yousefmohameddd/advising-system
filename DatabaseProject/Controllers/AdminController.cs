using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using DatabaseProject.Models;

namespace DatabaseProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
          
            string adminUsername = "admin";
            string adminPassword = "admin123";

            // Get user input from the form
            string username = form["username"];
            string password = form["password"];

            // Check if the entered credentials match the hardcoded admin credentials
            if (username == adminUsername && password == adminPassword)
            {
                // Admin login successful
                // You might want to set a session variable to indicate that the user is logged in
                Session["IsAdmin"] = true;

                // Redirect to the admin dashboard or any other admin-specific page
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                // Incorrect credentials, handle the error (e.g., display an error message)
                ViewBag.ErrorMessage = "Invalid username or password";
                return View("Login"); // Assuming you have a Login view
            }
        }

        // GET: Admin/AdminDashboard
        public ActionResult AdminDashboard()
        {
            if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
            {
                // Admin is authenticated, show the admin dashboard
                return View();
            }
            else
            {
                // Redirect to the login page if not authenticated as admin
                return RedirectToAction("Login");
            }
        }
        public ActionResult ListAdvisors()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Procedures_AdminListAdvisors", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);
        }
        public ActionResult StudentwithAdvisor()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AdminListStudentsWithAdvisors", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);
        }
        public ActionResult PendingRequests()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From all_Pending_Requests", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);
        }
        public ActionResult AddNewSem()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult AddNewSem(SemesterO viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("AdminAddingSemester", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@start_date", SqlDbType.Date)).Value = viewModel.Start_Date;
                            cmd.Parameters.Add(new SqlParameter("@end_date", SqlDbType.Date)).Value = viewModel.End_Date;
                            cmd.Parameters.Add(new SqlParameter("@semester_code", SqlDbType.NVarChar, 50)).Value = viewModel.SemesterCode;

                            con.Open();

                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were affected (insert successful)
                            if (rowsAffected > 0)
                            {
                                // Successfully added data
                                TempData["SuccessMessage"] = "Data added successfully!";
                                return RedirectToAction("AddNewSem"); // Redirect to the form again
                            }
                            else
                            {
                                // Handle the case where data was not added
                                ViewBag.ErrorMessage = "Failed to add data. Please try again.";
                            }
                        }
                    }
                }
                catch (SqlException )
                { 
                        ViewBag.ErrorMessage = "Error";
                   
                }

                // If an exception occurred, return the view with the provided error message
                return View(viewModel);
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }
        public ActionResult AddNewCourse()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddNewCourse(CourseO viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Procedures_AdminAddingCourse", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@major", SqlDbType.VarChar)).Value = viewModel.Major;
                            cmd.Parameters.Add(new SqlParameter("@semester", SqlDbType.Int)).Value = viewModel.Semester;
                            cmd.Parameters.Add(new SqlParameter("@credit_hours", SqlDbType.Int)).Value = viewModel.CreditHours;
                            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar)).Value = viewModel.CourseName;
                            cmd.Parameters.Add(new SqlParameter("@is_offered", SqlDbType.Bit)).Value = viewModel.Offered;
                            

                            con.Open();

                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were affected (insert successful)
                            if (rowsAffected > 0)
                            {
                                // Successfully added data
                                TempData["SuccessMessage"] = "Data added successfully!";
                                return RedirectToAction("AddNewCourse"); // Redirect to the form again
                            }
                            else
                            {
                                // Handle the case where data was not added
                                ViewBag.ErrorMessage = "Failed to add data. Please try again.";
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    ViewBag.ErrorMessage = "Error";

                }

                // If an exception occurred, return the view with the provided error message
                return View(viewModel);
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }
        public ActionResult LinkInstructorToCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LinkInstructorToCourse(LinkInstructorView viewModel)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Procedures_AdminLinkInstructor", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;



                            cmd.Parameters.Add(new SqlParameter("@instructor_id", SqlDbType.Int)).Value = viewModel.InstructorId;
                            cmd.Parameters.Add(new SqlParameter("@cours_id", SqlDbType.Int)).Value = viewModel.CourseId;
                            cmd.Parameters.Add(new SqlParameter("@slot_id", SqlDbType.Int)).Value = viewModel.SlotId;


                            con.Open();

                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were affected (insert successful)
                            if (rowsAffected > 0)
                            {
                                // Successfully added data
                                TempData["SuccessMessage"] = "Data Linked successfully!";
                                return RedirectToAction("LinkInstructorToCourse"); // Redirect to the form again
                            }
                            else
                            {
                                // Handle the case where data was not added
                                ViewBag.ErrorMessage = "Failed to add data. Please try again.";
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    ViewBag.ErrorMessage = "Error";

                }
                catch (Exception )
                {
                    ViewBag.ErrorMessage ="Diffrent Data type" ;
                }

                // If an exception occurred, return the view with the provided error message
                return View(viewModel);
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }
        public ActionResult LinkStudentToAdvisor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LinkStudentToAdvisor(LinkStudentToAdvisor viewModel)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Procedures_AdminLinkStudentToAdvisor", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;



                            cmd.Parameters.Add(new SqlParameter("@studentid", SqlDbType.Int)).Value = viewModel.StudentID;
                            cmd.Parameters.Add(new SqlParameter("@advisorid", SqlDbType.Int)).Value = viewModel.AdvisorID;
                            


                            con.Open();

                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were affected (insert successful)
                            if (rowsAffected > 0)
                            {
                                // Successfully added data
                                TempData["SuccessMessage"] = "Data Linked successfully!";
                                return RedirectToAction("LinkStudentToAdvisor"); // Redirect to the form again
                            }
                            else
                            {
                                // Handle the case where data was not added
                                ViewBag.ErrorMessage = "Failed to add data. Please try again.";
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    ViewBag.ErrorMessage = "Error";

                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "Diffrent Data type";
                }

                // If an exception occurred, return the view with the provided error message
                return View(viewModel);
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }
        public ActionResult LinkStudentToCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LinkStudentToCourse(LinkStudentToCourse viewModel)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Procedures_AdminLinkStudent", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;



                            cmd.Parameters.Add(new SqlParameter("@cours_id", SqlDbType.Int)).Value = viewModel.CourseID;
                            cmd.Parameters.Add(new SqlParameter("@instructor_id", SqlDbType.Int)).Value = viewModel.InstructorID;
                            cmd.Parameters.Add(new SqlParameter("@studentid", SqlDbType.Int)).Value = viewModel.StudentID;
                            cmd.Parameters.Add(new SqlParameter("@semester_code", SqlDbType.VarChar)).Value = viewModel.SemesterCode;



                            con.Open();

                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if any rows were affected (insert successful)
                            if (rowsAffected > 0)
                            {
                                // Successfully added data
                                TempData["SuccessMessage"] = "Data Linked successfully!";
                                return RedirectToAction("LinkStudentToCourse"); // Redirect to the form again
                            }
                            else
                            {
                                // Handle the case where data was not added
                                ViewBag.ErrorMessage = "Failed to add data. Please try again.";
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    ViewBag.ErrorMessage = "Error";

                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "Diffrent Data type";
                }

                // If an exception occurred, return the view with the provided error message
                return View(viewModel);
            }

            // If ModelState is not valid, return the view with validation errors
            return View(viewModel);
        }
        public ActionResult InstructorWithCourses()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From Instructors_AssignedCourses", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);
        }
        public ActionResult SemestersWithCourses()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From Semster_offered_Courses\r\n", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);
        }
        public ActionResult Delete_Course(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdminDeleteCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrEmpty(form["course_id"]))
                    {
                        TempData["ErrorMessage"] = "Course_id Space is empty , Please fill it!";
                    }


                    else
                    {
                        cmd.Parameters.AddWithValue("@courseID", form["course_id"]);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        TempData["SuccessMessage"] = "Delete operation was successful!";
                    }



                }
            }
            return Redirect("Delete_Coursev");
        }

        public ActionResult Delete_Slot(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdminDeleteSlots", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrEmpty(form["CS_Semester_Code"]))
                    {
                        TempData["ErrorMessage"] = "CS_Semester_Code Space is empty , Please fill it!";
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@current_semester", form["CS_Semester_Code"]);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        TempData["SuccessMessage"] = "Delete operation was successful!";

                    }

                }
            }
            return Redirect("Delete_Slot_View");
        }

        public ActionResult Add_Exam(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdminAddExam", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    try
                    {
                        if (String.IsNullOrEmpty(form["Type"]))
                        {
                            TempData["ErrorMessage"] = "Type Space is empty , Please fill it!";
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(form["Date"]))
                            {
                                TempData["ErrorMessage"] = "Date Space is empty , Please fill it!";
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(form["Course_Id"]))
                                {
                                    TempData["ErrorMessage"] = "Course_ID Space is empty , Please fill it!";
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@Type", form["Type"]);
                                    cmd.Parameters.AddWithValue("@date", form["Date"]);
                                    cmd.Parameters.AddWithValue("@courseID", form["Course_Id"]);
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    TempData["SuccessMessage"] = "Add operation was successful!";
                                }
                            }

                        }
                    }
                    catch (SqlException)
                    {
                        TempData["ErrorMessage"] = "Course_id is not right,try another one!";
                    }


                }
            }
            return Redirect("Add_Exam_View");
        }
        public ActionResult Student_Payment_View()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From Student_Payment", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);


        }
        public ActionResult Issue_Installments(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedures_AdminIssueInstallment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrEmpty(form["Payment_ID"]))
                    {
                        TempData["ErrorMessage"] = "Payment_ID Space is empty , Please fill it!";
                    }
                    else
                    {
                        try
                        {
                            cmd.Parameters.AddWithValue("@payment_id", form["Payment_ID"]);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            TempData["SuccessMessage"] = "Issue operation was successful!";
                        }
                        catch (SqlException)
                        {
                            TempData["ErrorMessage"] = "This Payment_ID was used before , Try different ID!";
                        }


                    }
                }
            }
            return Redirect("Issue_Insallments_View");
        }
        public ActionResult Update_Fianicial_Status(FormCollection form)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString);
            using (con)
            {
                SqlCommand cmd = new SqlCommand("dbo.Procedure_AdminUpdateStudentStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                using (cmd)
                {
                    if (String.IsNullOrEmpty(form["student_id"]))
                    {
                        TempData["ErrorMessage"] = "StudentID Space is empty , Please fill it!";
                    }

                    else
                    {
                        cmd.Parameters.AddWithValue("@student_id", form["student_id"]);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        TempData["SuccessMessage"] = "Update operation was successful!";

                    }
                }
                return Redirect("Update_Fiancial_Status_View");
            }
        }
        public ActionResult Active_Students_View()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From view_Students", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);


        }
        public ActionResult Graduation_plan_advi()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From Advisors_Graduation_Plan", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);


        }
        public ActionResult STCC()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From Students_Courses_transcript", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);


        }
        public ActionResult SOCC()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["team_2"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From Semster_offered_Courses", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Load the data into the DataTable
                    dt.Load(rdr);

                    con.Close();
                }
            }

            // Pass the DataTable directly to the view
            return View(dt);


        }


        public ActionResult Delete_Coursev()
        {
            return View();
        }

       
        public ActionResult Delete_Slot_View()
        {
            return View();
        }
        public ActionResult Add_Exam_View()
        {
            return View();
        }
        public ActionResult Issue_Insallments_View()
        {
            return View();
        }
        public ActionResult Update_Fiancial_Status_View()
        {
            return View();
        }




    }


}


