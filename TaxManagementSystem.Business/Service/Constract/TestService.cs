using System;
namespace TaxManagementSystem.Business.Service.Constract
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TaxManagementSystem.Business.Service.Concrete;
    using TaxManagementSystem.Core.DDD.Service;
    using TaxManagementSystem.Drms.DataBaseMode;
    using TaxManagementSystem.Model.Common;

    /// <summary>
    /// 测试服务实现  用于web调用服务
    /// </summary>
    public class TestService : ITestService
    {
        Result ITestService.TestFunction()
        {


            //业务处理


            return TestMode.AddTest("123");
        }
    }
}
