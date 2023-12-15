using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class Request
    {
        public Request(int request_id)
        {
            this.request_id = request_id;
        }

        public Request(int request_id, string type, string comment, string status,
                       int credit_hours, int course_id, Student student, Advisor advisor)
        {
            this.request_id = request_id;
            this.type = type;
            this.comment = comment;
            this.status = status;
            this.credit_hours = credit_hours;
            this.course_id = course_id;
            this.student = student;
            this.advisor = advisor;
        }
        public int request_id { get; set; }
        public string type { get; set; }
        public string comment { get; set; }
        public string status { get; set; }

        public int credit_hours { get; set; }

        public int course_id { get; set; }

        public Student student { get; set; }
        public Advisor advisor { get; set; }

    }
}
