using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace DatabaseM3.Models
{
    public class Slot
    {
        public int SlotId { get; set; }
        public String Day { get; set; }
        public String Time { get; set; }
        public String Location { get; set; }
        public int S_Course_id { get; set; }
        public int Instructor_id { get; set; }

    }
}