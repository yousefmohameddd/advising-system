using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class PendingRequests
    {
        public int RequestID { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; } = "Pending";
        public int CreditHours { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public int AdvisorID { get; set; }
    }
}