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
    }
}