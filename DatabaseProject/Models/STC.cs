using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseM3.Models
{
    public class STC
    {
        public int Student_id {  get; set; }
        public string Student_f_name { get; set; }
        public string Student_l_name {  get; set; }
        public int Course_id { get; set; }
        public string Course_name {  get; set; }
        public string Exam_type {  get; set; }
        public string Grade { get; set; }
        public string Semester_code {  get; set; }
    }
}