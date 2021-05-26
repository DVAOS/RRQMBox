using System;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using RRQMCore.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMRPC.RRQMTest
{
    public class Server
    {
        public Server(IRPCClient client)
        {
            this.Client = client;
        }
        public IRPCClient Client { get; private set; }
        public void TestNullReturnNullParameter(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            Client.Invoke("TestNullReturnNullParameter", invokeOption, ref parameters);
        }
        public async void TestNullReturnNullParameterAsync(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestNullReturnNullParameter(invokeOption);
            });
        }
        public System.String TestStringReturnNullParameter(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            System.String returnData = (System.String)Client.Invoke("TestStringReturnNullParameter", invokeOption, ref parameters);
            return returnData;
        }
        public async Task<System.String> TestStringReturnNullParameterAsync(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            return await Task.Run(() =>
            {
                return TestStringReturnNullParameter(invokeOption);
            });
        }
        public void TestNullReturnStringParameter(System.String name, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { name };
            Client.Invoke("TestNullReturnStringParameter", invokeOption, ref parameters);
        }
        public async void TestNullReturnStringParameterAsync(System.String name, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestNullReturnStringParameter(name, invokeOption);
            });
        }
        public void TestNullReturnOutStringParameter(out System.String name, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { default(System.String) };
            Client.Invoke("TestNullReturnOutStringParameter", invokeOption, ref parameters);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
            }
            else
            {
                name = default(System.String);
            }
        }
        public System.String TestStringReturnOutStringParameter(out System.String name, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { default(System.String) };
            System.String returnData = (System.String)Client.Invoke("TestStringReturnOutStringParameter", invokeOption, ref parameters);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
            }
            else
            {
                name = default(System.String);
            }
            return returnData;
        }
        public void TestNullReturnRefStringParameter(ref System.String name, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { name };
            Client.Invoke("TestNullReturnRefStringParameter", invokeOption, ref parameters);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
            }
        }
        public void TestNullReturnOutParameters(out System.String name, out System.Int32 age, out System.String occupation, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { default(System.String), default(System.Int32), default(System.String) };
            Client.Invoke("TestNullReturnOutParameters", invokeOption, ref parameters);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
                age = (System.Int32)parameters[1];
                occupation = (System.String)parameters[2];
            }
            else
            {
                name = default(System.String);
                age = default(System.Int32);
                occupation = default(System.String);
            }
        }
        public RRQMRPC.RRQMTest.Test02 TestClass1AndClass2(RRQMRPC.RRQMTest.Test01 test01, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { test01 };
            RRQMRPC.RRQMTest.Test02 returnData = (RRQMRPC.RRQMTest.Test02)Client.Invoke("TestClass1AndClass2", invokeOption, ref parameters);
            return returnData;
        }
        public async Task<RRQMRPC.RRQMTest.Test02> TestClass1AndClass2Async(RRQMRPC.RRQMTest.Test01 test01, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            return await Task.Run(() =>
            {
                return TestClass1AndClass2(test01, invokeOption);
            });
        }
        public void TestGetSocketClient(System.String iDToken, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { iDToken };
            Client.Invoke("TestGetSocketClient", invokeOption, ref parameters);
        }
        public async void TestGetSocketClientAsync(System.String iDToken, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestGetSocketClient(iDToken, invokeOption);
            });
        }
        public void TestCallBack(System.String iDToken, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { iDToken };
            Client.Invoke("TestCallBack", invokeOption, ref parameters);
        }
        public async void TestCallBackAsync(System.String iDToken, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestCallBack(iDToken, invokeOption);
            });
        }
        public System.String TestAsync(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            System.String returnData = (System.String)Client.Invoke("TestAsync", invokeOption, ref parameters);
            return returnData;
        }
        public async Task<System.String> TestAsyncAsync(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            return await Task.Run(() =>
            {
                return TestAsync(invokeOption);
            });
        }
        public System.Collections.Generic.List<RRQMRPC.RRQMTest.Test01> TestReturnList(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            System.Collections.Generic.List<RRQMRPC.RRQMTest.Test01> returnData = (System.Collections.Generic.List<RRQMRPC.RRQMTest.Test01>)Client.Invoke("TestReturnList", invokeOption, ref parameters);
            return returnData;
        }
        public async Task<System.Collections.Generic.List<RRQMRPC.RRQMTest.Test01>> TestReturnListAsync(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            return await Task.Run(() =>
            {
                return TestReturnList(invokeOption);
            });
        }
        public System.Collections.Generic.Dictionary<System.Int32, System.String> TestReturnDic(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            System.Collections.Generic.Dictionary<System.Int32, System.String> returnData = (System.Collections.Generic.Dictionary<System.Int32, System.String>)Client.Invoke("TestReturnDic", invokeOption, ref parameters);
            return returnData;
        }
        public async Task<System.Collections.Generic.Dictionary<System.Int32, System.String>> TestReturnDicAsync(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            return await Task.Run(() =>
            {
                return TestReturnDic(invokeOption);
            });
        }
        public void TestStringDefaultNullValue(System.String s = null, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { s };
            Client.Invoke("TestStringDefaultNullValue", invokeOption, ref parameters);
        }
        public async void TestStringDefaultNullValueAsync(System.String s = null, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestStringDefaultNullValue(s, invokeOption);
            });
        }
        public void TestStringDefaultValue(System.String s = "123123123", InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { s };
            Client.Invoke("TestStringDefaultValue", invokeOption, ref parameters);
        }
        public async void TestStringDefaultValueAsync(System.String s = "123123123", InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestStringDefaultValue(s, invokeOption);
            });
        }
        public void TestValueDefaultValue(System.Int32 a = 1234, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { a };
            Client.Invoke("TestValueDefaultValue", invokeOption, ref parameters);
        }
        public async void TestValueDefaultValueAsync(System.Int32 a = 1234, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestValueDefaultValue(a, invokeOption);
            });
        }
        public void TestDoubleValueDefaultValue(System.Double a = 1234.021, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { a };
            Client.Invoke("TestDoubleValueDefaultValue", invokeOption, ref parameters);
        }
        public async void TestDoubleValueDefaultValueAsync(System.Double a = 1234.021, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            await Task.Run(() =>
            {
                TestDoubleValueDefaultValue(a, invokeOption);
            });
        }
    }
}
