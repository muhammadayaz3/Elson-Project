using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsonProject.Models
{
    public class MenusModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public int MenusId { get; set; }
        public int AllowView { get; set; }
        public int AllowEdit { get; set; }
        public int AllowCreate { get; set; }
        public int AllowDelte { get; set; }
    }
}