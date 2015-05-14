using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Student
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EGN { get; set; }
        public Boolean Gender { get; set; }
        public Boolean State{ get; set; }
    }
}