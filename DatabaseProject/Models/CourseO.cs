using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class CourseO
    {
        public string Major { get; set; }
        public int Semester { get; set; }
        public int CreditHours { get; set; }
        public string CourseName { get; set; }
        public bool Offered { get; set; }
    }
}