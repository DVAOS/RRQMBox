using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RRQMSocket.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
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

        [HttpGet("/[controller]/[action]")]
        [RRQMSocket.RPC.WebApi.HttpGet]
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


        RpcService IServerProvider.RpcService { get; set; }
        void IServerProvider.RpcEnter(IRpcParser parser, MethodInvoker methodInvoker, MethodInstance methodInstance)
        {

        }

        void IServerProvider.RpcError(IRpcParser parser, MethodInvoker methodInvoker, MethodInstance methodInstance)
        {

        }

        void IServerProvider.RpcLeave(IRpcParser parser, MethodInvoker methodInvoker, MethodInstance methodInstance)
        {

        }
    }
}
