using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentRanking.Models
{
    public class Preference
    {
        //[Column(TypeName = "string")]
        [Required]
        [Key, Column(Order = 0)]
        public string EGN{ get; set; }

        //[Column(TypeName = "string")]
        [Required]
        public string ProgrammeName{ get; set; }

        //[Column(TypeName = "integer")]
        [Required]
        [Key, Column(Order = 1)]
        public int PrefNumber{ get; set; }

        //[Column(TypeName = "datetime2")]
        [Required]
        public bool IsApproved { get; set; }
    }
}