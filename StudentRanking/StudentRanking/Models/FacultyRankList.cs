using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class FacultyRankList
    {
        [Key]
        [Column(Order = 0)]
        public String ProgrammeName { get; set; }
        [Key]
        [Column(Order = 1)]
        public String EGN { get; set; }
        public Double TotalGrade { get; set; }
    }
}