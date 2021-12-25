//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using CookComputing.XmlRpc;

namespace RRQMBox.Client.Common
{
    public class XmlRPCv2
    {
        public static string GetMsg()
        {
            IClient iclient;
            XmlRpcClientProtocol protocol;
            iclient = (IClient)XmlRpcProxyGen.Create(typeof(IClient));
            protocol = (XmlRpcClientProtocol)iclient;
            protocol.Url = "http://127.0.0.1:7802";
            protocol.KeepAlive = false;

            string mes = iclient.Test20_XmlRpc("test", 10, 10.00, new Args[] { new Args() { P3 = "P" }, new Args() { P3 = "PP" } }); //调用
            return mes;
        }
    }

    public interface IClient
    {
        [XmlRpcMethod("Test20_XmlRpc")]
        string Test20_XmlRpc(string param, int a, double b, Args[] args);
    }

    public class Args
    {
        public System.Int32 P1 { get; set; }
        public System.Double P2 { get; set; }
        public System.String P3 { get; set; }
    }
}