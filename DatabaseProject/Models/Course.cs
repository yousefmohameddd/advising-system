using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class Course
    {
        public Course()
        {
            
        }

        public Course(string course_name)
        {
            this.name = course_name;
        }

        public Course(int course_id)
        {
            this.course_id = course_id;
        }
        public Course(int course_id  , string name )
        {
           this.course_id = course_id;
           this.name = name;
        }

         public int course_id { get; set; }
         public string name {  get; set; }
         public string major {  get; set; } 
         public bool is_offered {  get; set; }
         public int credit_hours {  get; set; }
         public int semester {  get; set; }
    }
}