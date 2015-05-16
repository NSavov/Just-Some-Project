using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Exam
    {
        [Key][Column(Order=0)]
        public String ExamName{ get; set; }
        [Key][Column(Order = 1)]
        public String StudentEGN{ get; set; }
        public double Grade{ get; set; }
    }
}