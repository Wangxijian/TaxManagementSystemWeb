namespace TaxManagementSystem.Drms.DataBaseMode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TaxManagementSystem.Core.Data;
    using TaxManagementSystem.Drms.Spefication;
    using TaxManagementSystem.Model.Common;
    using TaxManagementSystem.Model.Data;


    /// <summary>
    /// 测试模型 数据处理部分
    /// </summary>
    public static class TestMode
    {


        public static Result AddTest(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("测试添加方法 参数名value 不能为空");
            }

            Result result = new Result();
            result.Status = false;
            result.Message = "处理中";
            try
            {
                //每次调用实例化一个连接
                //读取
                MysqlDBReader reader = new MysqlDBReader();
                //写入 创建写入伴随的是开启事务 写入完成 要提交
                MysqlDBWriter writer = new MysqlDBWriter();

                //查询数据
                IList<TestInfo> testlist = reader.Select<TestInfo>(new TestSelectSpefication(value).Satifasy());
                //写入数据
                //step 1 业务处理
                bool next = false;

                next = writer.Insert("新增sql 语句");

                if (next)
                {
                    //step 2 业务处理
                    next = writer.Update("修改 sql 语句");
                }

                //step 3 业务处理完成

                result.Status = next;

                if (result.Status)
                {
                    result.Message = "处理成功";
                    //业务处理完 提交事务
                    writer.Commit();
                }
                else
                {
                    result.Message = "处理失败";
                    //业务处理完 回滚事务
                    writer.Rollback();
                }



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
