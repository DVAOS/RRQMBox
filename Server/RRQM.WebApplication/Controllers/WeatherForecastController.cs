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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RRQM.WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase, IServerProvider
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public RPCService RPCService { get; set; }

        [JsonRpc]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [NonAction]
        public void RPCEnter(IRPCParser parser, MethodInvoker methodInvoker, MethodInstance methodInstance)
        {
        }

        [NonAction]
        public void RPCError(IRPCParser parser, MethodInvoker methodInvoker, MethodInstance methodInstance)
        {
        }

        [NonAction]
        public void RPCLeave(IRPCParser parser, MethodInvoker methodInvoker, MethodInstance methodInstance)
        {
        }
    }
}