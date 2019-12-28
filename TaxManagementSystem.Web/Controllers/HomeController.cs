using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxManagementSystem.Business.Service.Concrete;
using TaxManagementSystem.Core.DDD.Service;
using TaxManagementSystem.Model.Common;

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

            //获取测试服务
            ITestService service = ServiceObjectContainer.Get<ITestService>();

            Result result = service.TestFunction();

            
            return View();
        }

        public JsonResult Getsss()
        {
            ITestService service = ServiceObjectContainer.Get<ITestService>();

            Result result = service.TestFunction();

            return Json(result);
        }

    }
}