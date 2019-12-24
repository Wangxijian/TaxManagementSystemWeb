using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TaxManagementSystem.Core.Utilits
{
    public class IPAddressUnit
    {
        public static string GetIpv4Address()
        {
            string localaddr = "";

            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostEntry(hostName);

            for (int i = 0; i < localhost.AddressList.Length; i++)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (localhost.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    localaddr = localhost.AddressList[i].ToString();
                }
            }
            return localaddr;
        }

    }
}
