using ElsonProject.Models;
using Newtonsoft.Json;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ElsonProject.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        dynamic AppDb = Database.OpenNamedConnection("MainDB");
        public ActionResult CustomerMaster()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult ChartofAccount()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ChartofAccount(ChartOfAccount data, string compcodes)
        {
            var str=compcodes.Split(',');
            dynamic Records;
            for (int i = 0; i < str.Length; i++)
            {
                Records = AppDb.WEBInsertUpdate(AcctCode: data.AccountCode, AcctDesc: data.Description, Acctype: 01, Validact: 1, ParentCode: data.ParentCode, Lv: data.Levelno, PNLCode: data.PNLCode, BSCode: data.BSCode).FirstOrDefault();
            }

            return View();
        }

        public ActionResult _AddChartOfAccount()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult ChartAccount(string CampId=null)
        {
            try
            {
                
                var records = AppDb.WEB_GetChartofAccont(CompCode: CampId);
                records.FirstOrDefault();

                var recolist = records.ToList<dynamic>();

                var treeData = BuildTree(recolist);
                records.NextResult();

                var companys = records.ToList<dynamic>();

                return Json(new { data = treeData,company=companys }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<JsTreeItem> BuildTree(List<dynamic> records)
        {
            var tree = new Dictionary<string, JsTreeItem>();

            foreach (var item in records)
            {
                tree[item.AcctCode] = new JsTreeItem
                {
                    AcctCode = item.AcctCode,
                    AcctDesc = item.AcctDesc,
                    ParentCode = item.ParentCode,
                    Children = new List<JsTreeItem>()
                };
            }

            List<JsTreeItem> rootNodes = new List<JsTreeItem>();
            foreach (var item in records)
            {
                if (item.ParentCode == "0") 
                {
                    rootNodes.Add(tree[item.AcctCode]);
                }
                else
                {
                    if (tree.ContainsKey(item.ParentCode))
                    {
                        tree[item.ParentCode].Children.Add(tree[item.AcctCode]);
                    }
                }
            }

            return rootNodes;
        }


        public ActionResult UMO(int Id = 1)
        {
            try
            {
                dynamic AppDB = Database.OpenNamedConnection("MainDB");
                List<UmoModel> Records = new List<UmoModel>();
                dynamic Record = AppDB.WEB_GETUMO(PageIndex:Id);

                ViewBag.PageNumber = Id;
                if (Record.FirstOrDefault() != null)
                {
                    Records = Record.ToList<UmoModel>();
                }
                int min = 0;
                if (Records.Count < 25) { min = 25 - Records.Count; };
                int a = 25 * Id;

                if (min > 0) { ViewBag.CurrentRecords = a - min; }
                else { ViewBag.CurrentRecords = a; }

                Record.NextResult();

                ViewBag.totalPages = Record.FirstOrDefault().totalpages;
                ViewBag.TotalRows = Record.FirstOrDefault().TotalRows;

                return View(Records);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult _EditUMO(int? Id = null)
        {
            UmoModel user = null;
            if (Id != null)
            {
                dynamic AppDB = Database.OpenNamedConnection("MainDB");
                user = AppDB.WEB_GETUMO(UMOCode:Id).FirstOrDefault();
                ViewBag.UserId = user.UMOCode;
                TempData["status"] = user.Active;
            }
            else
            {
                ViewBag.UserId = "0";
                TempData["status"] = "0";
            }
            return PartialView(user);
        }

        [HttpPost]
        public ActionResult UMO(UmoModel user)
        {
            try
            {
                dynamic Records = null;
                if (user.UMOCode > 0)
                {
                    Records = AppDb.WEB_UpdateUMO(UMOName: user.UMOName, UMOCode: user.UMOCode, Status: user.Active).FirstOrDefault();
                }
                else
                {
                    Records = AppDb.WEB_AddUMO(UMOName: user.UMOName, Status: user.Active, CompCode: 0001, UserId: 1).FirstOrDefault();
                }
                TempData["status"] = "toast-success";
                TempData["message"] = Records.Message;
            }
            catch (Exception ex)
            {
                TempData["status"]= "toast-error";
                TempData["message"]="Internal Server Error";
            }
            return RedirectToAction("UMO");
        }
        public ActionResult _AddUMO()
        {
            return PartialView();
        }

        public ActionResult SuppilerMaster()
        {
            return View();
        }
    }
}