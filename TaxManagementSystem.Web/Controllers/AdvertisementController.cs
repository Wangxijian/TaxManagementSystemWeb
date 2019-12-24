using System;
namespace TaxManagementSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using TaxManagementSystem.Business.Service.Concrete;
    using TaxManagementSystem.Core.DDD.Service;
    using TaxManagementSystem.Model.Common;
    using TaxManagementSystem.Model.Show;

    /// <summary>
    /// API 控制器
    /// </summary>
    public class AdvertisementController : ApiController
    {

        /// <summary>
        /// 测试方法 API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result TestFunction()
        {

            //获取测试服务
            ITestService service = ServiceObjectContainer.Get<ITestService>();

            Result result = service.TestFunction();

            return result;

        }


        /// <summary>
        /// 获取广告列表api
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result<TestInfo> GetAdvertisementList()
        {
            
            Result<TestInfo> result = new Result<TestInfo>();


            result.Status = false;
            result.Data = new TestInfo();
            result.Data.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            result.Message = "获取数据成功";


            return result;
        }

    }
}
