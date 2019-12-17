using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxManagementSystem.Web.Controllers
{

    /// <summary>
    /// 主页页面控制器
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// 主页页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        
    }
}