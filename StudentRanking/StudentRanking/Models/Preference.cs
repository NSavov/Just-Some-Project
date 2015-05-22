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
        [Required]
        [Key]
        [Column(Order = 0)]
        public String EGN{ get; set; }

        [Required]
        public String ProgrammeName{ get; set; }

        [Required]
        [Key]
        [Column(Order = 1)]
        public int PrefNumber{ get; set; }

        [Required]
        public Boolean IsApproved { get; set; }
    }
}