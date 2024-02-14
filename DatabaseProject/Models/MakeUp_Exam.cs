using DatabaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class MakeUp_Exam
    {
        public MakeUp_Exam(string cname, int semester, int exam_id, string date, string type)
        {
            this.course = new Course();
            this.course.name = cname;
            this.course.semester = semester;
            this.exam_id = exam_id;
            this.date = date;
            this.type = type;
        }

        public int exam_id { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public Course course { get; set; }
    }
}