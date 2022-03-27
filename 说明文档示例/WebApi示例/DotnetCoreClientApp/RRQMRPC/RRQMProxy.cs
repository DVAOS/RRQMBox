using System;
using RRQMCore;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMProxy
{
    public interface IWeatherForecastController : IRemoteServer
    {
        ///<summary>
        ///无注释信息
        ///</summary>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        System.Collections.Generic.IEnumerable<WeatherForecast> Get(IInvokeOption invokeOption = default);
        ///<summary>
        ///无注释信息
        ///</summary>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        Task<System.Collections.Generic.IEnumerable<WeatherForecast>> GetAsync(IInvokeOption invokeOption = default);

    }
    public class WeatherForecastController : IWeatherForecastController
    {
        public WeatherForecastController(IRpcClient client)
        {
            this.Client = client;
        }
        public IRpcClient Client { get; private set; }
        ///<summary>
        ///<inheritdoc/>
        ///</summary>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        public System.Collections.Generic.IEnumerable<WeatherForecast> Get(IInvokeOption invokeOption = default)
        {
            if (Client == null)
            {
                throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            System.Collections.Generic.IEnumerable<WeatherForecast> returnData = Client.Invoke<System.Collections.Generic.IEnumerable<WeatherForecast>>("GET:WeatherForecast/Get", invokeOption, parameters);
            return returnData;
        }
        ///<summary>
        ///<inheritdoc/>
        ///</summary>
        /// <exception cref="TimeoutException">调用超时</exception>
        /// <exception cref="RpcSerializationException">序列化异常</exception>
        /// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
        /// <exception cref="RRQMException">其他异常</exception>
        public Task<System.Collections.Generic.IEnumerable<WeatherForecast>> GetAsync(IInvokeOption invokeOption = default)
        {
            if (Client == null)
            {
                throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            return Client.InvokeAsync<System.Collections.Generic.IEnumerable<WeatherForecast>>("GET:WeatherForecast/Get", invokeOption, parameters);
        }

    }

    public class WeatherForecast
    {
        public System.DateTime Date { get; set; }
        public System.Int32 TemperatureC { get; set; }
        public System.Int32 TemperatureF { get; set; }
        public System.String Summary { get; set; }
    }

}
