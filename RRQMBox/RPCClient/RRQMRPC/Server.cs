using System.Collections.Generic;
using System.Threading.Tasks;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;

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
            Client.RPCInvoke("TestNullReturnNullParameter", ref parameters, invokeOption);
            if (parameters != null)
            {
            }
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
            System.String returnData = Client.RPCInvoke<System.String>("TestStringReturnNullParameter", ref parameters, invokeOption);
            if (parameters != null)
            {
            }
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
            Client.RPCInvoke("TestNullReturnStringParameter", ref parameters, invokeOption);
            name = default(System.String);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
            }
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
            Client.RPCInvoke("TestNullReturnOutStringParameter", ref parameters, invokeOption);
            name = default(System.String);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
            }
        }

        public System.String TestStringReturnOutStringParameter(out System.String name, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { default(System.String) };
            System.String returnData = Client.RPCInvoke<System.String>("TestStringReturnOutStringParameter", ref parameters, invokeOption);
            name = default(System.String);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
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
            Client.RPCInvoke("TestNullReturnRefStringParameter", ref parameters, invokeOption);
            name = default(System.String);
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
            Client.RPCInvoke("TestNullReturnOutParameters", ref parameters, invokeOption);
            name = default(System.String);
            age = default(System.Int32);
            occupation = default(System.String);
            if (parameters != null)
            {
                name = (System.String)parameters[0];
                age = (System.Int32)parameters[1];
                occupation = (System.String)parameters[2];
            }
        }

        public RRQMRPC.RRQMTest.Test02 TestClass1AndClass2(RRQMRPC.RRQMTest.Test01 test01, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { test01 };
            RRQMRPC.RRQMTest.Test02 returnData = Client.RPCInvoke<RRQMRPC.RRQMTest.Test02>("TestClass1AndClass2", ref parameters, invokeOption);
            test01 = default(RRQMRPC.RRQMTest.Test01);
            if (parameters != null)
            {
                test01 = (RRQMRPC.RRQMTest.Test01)parameters[0];
            }
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
            Client.RPCInvoke("TestGetSocketClient", ref parameters, invokeOption);
            iDToken = default(System.String);
            if (parameters != null)
            {
                iDToken = (System.String)parameters[0];
            }
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
            Client.RPCInvoke("TestCallBack", ref parameters, invokeOption);
            iDToken = default(System.String);
            if (parameters != null)
            {
                iDToken = (System.String)parameters[0];
            }
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
            System.String returnData = Client.RPCInvoke<System.String>("TestAsync", ref parameters, invokeOption);
            if (parameters != null)
            {
            }
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

        public List<RRQMRPC.RRQMTest.Test01> TestReturnList(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            List<RRQMRPC.RRQMTest.Test01> returnData = Client.RPCInvoke<List<RRQMRPC.RRQMTest.Test01>>("TestReturnList", ref parameters, invokeOption);
            if (parameters != null)
            {
            }
            return returnData;
        }

        public async Task<List<RRQMRPC.RRQMTest.Test01>> TestReturnListAsync(InvokeOption invokeOption = null)
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

        public Dictionary<System.Int32, System.String> TestReturnDic(InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { };
            Dictionary<System.Int32, System.String> returnData = Client.RPCInvoke<Dictionary<System.Int32, System.String>>("TestReturnDic", ref parameters, invokeOption);
            if (parameters != null)
            {
            }
            return returnData;
        }

        public async Task<Dictionary<System.Int32, System.String>> TestReturnDicAsync(InvokeOption invokeOption = null)
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
            Client.RPCInvoke("TestStringDefaultNullValue", ref parameters, invokeOption);
            s = default(System.String);
            if (parameters != null)
            {
                s = (System.String)parameters[0];
            }
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
            Client.RPCInvoke("TestStringDefaultValue", ref parameters, invokeOption);
            s = default(System.String);
            if (parameters != null)
            {
                s = (System.String)parameters[0];
            }
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
            Client.RPCInvoke("TestValueDefaultValue", ref parameters, invokeOption);
            a = default(System.Int32);
            if (parameters != null)
            {
                a = (System.Int32)parameters[0];
            }
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
            Client.RPCInvoke("TestDoubleValueDefaultValue", ref parameters, invokeOption);
            a = default(System.Double);
            if (parameters != null)
            {
                a = (System.Double)parameters[0];
            }
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