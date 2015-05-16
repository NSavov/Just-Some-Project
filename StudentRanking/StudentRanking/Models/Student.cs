using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Student
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }

        [Key]
        public String EGN { get; set; }
        public String Email { get; set; }
        public bool? Gender { get; set; }
        public Boolean State{ get; set; }
    }
}