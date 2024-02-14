using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class Student_Instructor_Course_Take
    {
        public Student student{ get; set; }
        public Course course { get; set; }
        public Instructor instructor { get; set; }

        public string semester_code { get; set; }
        public string exam_type { get; set; }
        public string grade {  get; set; }




    }
}