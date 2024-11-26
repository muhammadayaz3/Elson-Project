using ElsonProject.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsonProject.Codebase
{
    public class AccountDBHandler
    {
        public UsersModel UserLogin(string username, string password,string Companyid)
        {

            var AppDB = Database.OpenNamedConnection("MainDB");
            var result = AppDB.WEB_LoginUser(Username: username, Password: password,CompanyId: Companyid).FirstOrDefault();

            if (result != null)
            {
                return result;
            }
            else
                return null;
        }
    }
}