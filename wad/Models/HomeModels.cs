using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wad.Models
{
    public class HomeModel
    {
        [Required]
        [Display(Name = "Url")]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Type")]
        public TypeModel Type { get; set; }
    }

    public enum TypeModel
    {
        MicroData = 0,
        MicroFormat = 1,
        Rdfa = 2
    }
}