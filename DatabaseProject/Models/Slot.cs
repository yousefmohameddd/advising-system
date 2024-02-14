using DatabaseProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class Slot
    {
        public int slot_id { get; set; }
        public string day { get; set; }
        public string time { get; set; }
        public string location { get; set; }
        public Course course { get; set; }
        public Instructor instructor { get; set; }

        public Slot(int slot_id, string day, string time, string location, string cname, string iname)
        {
            this.slot_id = slot_id;
            this.day = day;
            this.time = time;
            this.location = location;
            this.course.name = cname;
            this.instructor.name = iname;
        }
        public Slot(int id, string cname, int slot_id, string day, string time, string location, string iname)
        {


            this.course = new Course();
            this.instructor = new Instructor();
            this.course.course_id = id;
            this.course.name = cname;
            this.slot_id = slot_id;
            this.day = day;
            this.time = time;
            this.location = location;
            this.instructor.name = iname;
            if (this.course.name != null)
            {
                this.course.name = cname;
            }
            if (this.instructor.name != null)
            {
                this.instructor.name = iname;
            }

        }

    }
}