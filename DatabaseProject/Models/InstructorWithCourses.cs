using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace database.Models
{
    public class InstructorWithCourses
    {
        public int InstructorID { get; set; }
        public string InstructorName { get; set; }
        public int CourseID { get; set; }
        public string Coursename { get; set; }
    }
}