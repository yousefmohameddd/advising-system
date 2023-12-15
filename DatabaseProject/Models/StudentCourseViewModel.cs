using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class StudentCourseViewModel
    {
        public Student student { get; set; }
        public Course course { get; set; }
        
        public StudentCourseViewModel(Student student, Course course)
        {
            this.student = student;
            this.course = course;
        }


    }
}