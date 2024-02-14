using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class Instructor
    {
        public int instructor_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string faculty { get; set; }
        public string office { get; set; }
    }
}