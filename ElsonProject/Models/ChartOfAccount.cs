using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsonProject.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }
        public string ParentCode { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public string PNLCode { get; set; }
        public string BSCode { get; set; }
        public int Levelno { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}