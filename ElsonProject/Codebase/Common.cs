using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElsonProject.Codebase
{
    public class Common
    {

        public static IEnumerable<SelectListItem> GetBSCode()
        {
            var AppDB = Database.OpenNamedConnection("MainDB");
            IEnumerable<dynamic> records = AppDB.BSCode.All().ToList<dynamic>();
            var result = records.Select(x => new SelectListItem
            {
                Value = x.BSCode.ToString(),
                Text = x.BSDesc
            }).ToList();

            return result;

        }
        
        public static IEnumerable<SelectListItem> GetPNLCode()
        {
            var AppDB = Database.OpenNamedConnection("MainDB");
            IEnumerable<dynamic> records = AppDB.PNLCode.All().ToList<dynamic>();
            var result = records.Select(x => new SelectListItem
            {
                Value = x.PNLCode.ToString(),
                Text = x.PNLDesc
            }).ToList();

            return result;

        }
    }
}