using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Faculty
    {
        [Key]
        public String ProgrammeName{ get; set; }
        public String FacultyName{ get; set; }
    }
}