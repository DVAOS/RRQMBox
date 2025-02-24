﻿using RRQMSocket.RPC;
using System.Threading.Tasks;
namespace RRQMProxy
{
    public interface IMyRpcServer : IRemoteServer
    {
        ///<summary>
        ///登录
        ///</summary>
        /// <exception cref="System.TimeoutException">调用超时</exception>
        /// <exception cref="RRQMSocket.RPC.RRQMRpcInvokeException">Rpc调用异常</exception>
        /// <exception cref="RRQMCore.RRQMException">其他异常</exception>
        System.Boolean Login(System.String account, System.String password, IInvokeOption invokeOption = default);
        ///<summary>
        ///登录
        ///</summary>
        /// <exception cref="System.TimeoutException">调用超时</exception>
        /// <exception cref="RRQMSocket.RPC.RRQMRpcInvokeException">Rpc调用异常</exception>
        /// <exception cref="RRQMCore.RRQMException">其他异常</exception>
        Task<System.Boolean> LoginAsync(System.String account, System.String password, IInvokeOption invokeOption = default);

    }
    public class MyRpcServer : IMyRpcServer
    {
        public MyRpcServer(IRpcClient client)
        {
            this.Client = client;
        }
        public IRpcClient Client { get; private set; }
        ///<summary>
        ///登录
        ///</summary>
        /// <exception cref="System.TimeoutException">调用超时</exception>
        /// <exception cref="RRQMSocket.RPC.RRQMRpcInvokeException">Rpc调用异常</exception>
        /// <exception cref="RRQMCore.RRQMException">其他异常</exception>
        public System.Boolean Login(System.String account, System.String password, IInvokeOption invokeOption = default)
        {
            if (Client == null)
            {
                throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { account, password };
            System.Boolean returnData = Client.Invoke<System.Boolean>("myrpcserver/login", invokeOption, parameters);
            return returnData;
        }
        ///<summary>
        ///登录
        ///</summary>
        public Task<System.Boolean> LoginAsync(System.String account, System.String password, IInvokeOption invokeOption = default)
        {
            if (Client == null)
            {
                throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { account, password };
            return Client.InvokeAsync<System.Boolean>("myrpcserver/login", invokeOption, parameters);
        }

    }
    public static class MyRpcServerExtensions
    {
        ///<summary>
        ///登录
        ///</summary>
        /// <exception cref="System.TimeoutException">调用超时</exception>
        /// <exception cref="RRQMSocket.RPC.RRQMRpcInvokeException">Rpc调用异常</exception>
        /// <exception cref="RRQMCore.RRQMException">其他异常</exception>
        public static System.Boolean Login<TClient>(this TClient client, System.String account, System.String password, IInvokeOption invokeOption = default) where TClient :
        RRQMSocket.RPC.IRpcClient
        {
            if (client.TryCanInvoke?.Invoke(client) == false)
            {
                throw new RpcException("Rpc无法执行。");
            }
            object[] parameters = new object[] { account, password };
            System.Boolean returnData = client.Invoke<System.Boolean>("myrpcserver/login", invokeOption, parameters);
            return returnData;
        }
        ///<summary>
        ///登录
        ///</summary>
        public static Task<System.Boolean> LoginAsync<TClient>(this TClient client, System.String account, System.String password, IInvokeOption invokeOption = default) where TClient :
        RRQMSocket.RPC.IRpcClient
        {
            if (client.TryCanInvoke?.Invoke(client) == false)
            {
                throw new RpcException("Rpc无法执行。");
            }
            object[] parameters = new object[] { account, password };
            return client.InvokeAsync<System.Boolean>("myrpcserver/login", invokeOption, parameters);
        }

    }
}
