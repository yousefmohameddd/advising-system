using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class LinkInstructorView
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public int SlotId { get; set; }
    }
}