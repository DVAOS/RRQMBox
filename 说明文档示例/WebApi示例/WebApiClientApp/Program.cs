﻿using RRQMCore.IO;
using RRQMProxy;
using RRQMSocket.RPC.WebApi;
using System;

namespace WebApiClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = CreateWebApiClient();

            int sum1 = client.Invoke<int>("GET:/Server/Sum?a={0}&b={1}", null, 10, 20);
            Console.WriteLine($"Get调用成功，结果：{sum1}");

            int sum2 = client.Invoke<int>("POST:/Server/TestPost", null, new MyClass() { A = 10, B = 20 });
            Console.WriteLine($"Post调用成功，结果：{sum2}");

            ServerController serverController = new ServerController(client);
            int sum3 = serverController.TestPost(new MyClass() { A = 10, B = 20 });
            Console.WriteLine($"代理调用成功，结果：{sum3}");
        }

        private static WebApiClient CreateWebApiClient()
        {
            WebApiClient client = new WebApiClient();
            client.Setup("127.0.0.1:7789");
            client.Connect();
            Console.WriteLine("连接成功");
            return client;
        }
    }
}
