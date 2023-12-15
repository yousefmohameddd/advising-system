using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class Semester
    {
        public Semester(string semester_code)
        {
            this.semester_code = semester_code;
        }
        public string semester_code {  get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

    }
}