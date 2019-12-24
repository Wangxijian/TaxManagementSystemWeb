namespace TaxManagementSystem.Business.Handle
{
    using System;
    using Core.DDD.Hub;
    using TaxManagementSystem.Drms.DataBaseMode;
    using TaxManagementSystem.Model.Common;


    /// <summary>
    /// 测试集线器
    /// </summary>
    [Hub(Name = "处理测试 handle用于处理sockect通信相关业务", Condition1 = 0, Condition2 = 0, Condition3 = 0, Condition4 = "test")]
    public class TestHandle : IHub<string, Result>
    {
        public Result Handle(string obj)
        {
            Result result = new Result()
            {
                Status = false,
                Message = "处理失败"
            };

            try
            {

                //业务处理

                Result addresult = TestMode.AddTest(obj);

                result = addresult;

            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;

            }


            return result;




        }
    }
}
