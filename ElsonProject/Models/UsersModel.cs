using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ElsonProject.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int UserType  { get; set; }
        public bool AcctType  { get; set; }
        public int Status  { get; set; }
        public string Reason  { get; set; }
        public int CreatedDateTime  { get; set; }
    }
    public class JsTreeItem
    {
        public string AcctCode { get; set; }
        public string AcctDesc { get; set; }
        public string ParentCode { get; set; }
        public List<JsTreeItem> Children { get; set; }
    }
    public class JsTreeData
    {
        public string path { get; set; }
    }

}