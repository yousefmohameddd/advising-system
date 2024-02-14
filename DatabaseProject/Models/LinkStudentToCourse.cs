using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class LinkStudentToCourse
    {
        
       
        
        public int InstructorID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public string SemesterCode { get; set; }

    }
}