using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxManagementSystem.Drms.Spefication
{
    public class TestSelectSpefication : SpeficationBase
    {
        private string _value;

        /// <summary>
        /// 构造函数 创建测试规约
        /// </summary>
        /// <param name="value">入参数</param>
        public TestSelectSpefication(string value)
        {
            _value = value;
        }




        public override string Satifasy()
        {
            string sql = string.Format(string.Format(" select * from test where name = '{0}'",_value));

            return sql;

        }
    }
}
