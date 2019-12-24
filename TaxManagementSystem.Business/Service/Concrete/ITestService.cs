namespace TaxManagementSystem.Business.Service.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TaxManagementSystem.Core.DDD.Service;
    using TaxManagementSystem.Model.Common;

    /// <summary>
    /// 测试服务具象
    /// </summary>
    public interface ITestService : IServiceBase
    {

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        Result TestFunction();

    }
}
