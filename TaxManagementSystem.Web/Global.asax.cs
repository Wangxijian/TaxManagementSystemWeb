using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaxManagementSystem.Core.Data.Connection;
using TaxManagementSystem.Core.DDD.Hub;
using TaxManagementSystem.Core.DDD.Service;

namespace TaxManagementSystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //准备
            Prepared();

        }



        protected void Prepared()
        {

            MysqlDBConnection.Current.Database = "";
            MysqlDBConnection.Current.Server = "localhost";
            MysqlDBConnection.Current.LoginUser = "";
            MysqlDBConnection.Current.Password = "";

            //数据库部署
            MysqlDBConnection.Current.Deployment();

            //集线器加载
            HubContainer.Load(Assembly.Load("TaxManagementSystem.Business"));
            //服务加载
            ServiceObjectContainer.Load(Assembly.Load("TaxManagementSystem.Business"));
            

        }


    }
}
