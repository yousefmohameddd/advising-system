using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseProject.Models
{
    public class RequestSemesterModel
    {
        public Request request;
        public Semester semester;

        public RequestSemesterModel(Request request, Semester semester)
        {
            this.request = request;
            this.semester = semester;
            
        }
    }
}