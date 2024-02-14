using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class GradPlanCourse
    {

        public Course course { get; set; }
        public Semester semester { get; set; }
        public Graduation_Plan gradPlan { get; set; }

        public GradPlanCourse()
        {

        }
        public GradPlanCourse(int id, string semester_code, int course_id)
        {
            // Initialize plan and course objects
            this.gradPlan = new Graduation_Plan();
            this.course = new Course();

            this.gradPlan.plan_id = id;
            this.gradPlan.semester_code = semester_code;
            this.course.course_id = course_id;
        }
    }
}