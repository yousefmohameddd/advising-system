using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace DatabaseM3.Models
{
    public class Student_Payment
    {
        public int Student_Id { get; set; }
        public string Student_F_Name { get; set; }
        public string Student_L_Name { get; set; }
        public int Payment_id { get; set; }
        public int amount { get; set; }
        public DateTime Deadline { get; set; }
        public int n_installments { get; set; }
        public string Status { get; set; }
        public decimal Fund_Percentage { get; set; }
        public DateTime Start_Date { get; set; }
        public string Semester_code { get; set; }
    }
}