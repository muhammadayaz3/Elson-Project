using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElsonProject.Models
{
    public class UmoModel
    {
        public string CompCode { get; set; }
        public int UMOCode { get; set; }

        [Required]
        public string UMOName { get; set; }
        public string UserId { get; set; }
        public int Active { get; set; }
        public string CreatedDateTime { get; set; }
    }
}