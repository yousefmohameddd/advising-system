using DatabaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class PrerequisitesCourses
    {
        public PrerequisitesCourses()
        {

        }
        public PrerequisitesCourses(int course_id, int prerequisite_course_id)
        {

            this.course = new Course();
            this.Prerequisite = new Course();
            this.course.course_id = course_id;
            this.Prerequisite.course_id = prerequisite_course_id;
        }

        public Course course { get; set; }

        public Course Prerequisite { get; set; }
    }
}