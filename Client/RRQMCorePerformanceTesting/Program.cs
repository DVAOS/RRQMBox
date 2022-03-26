//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;
using RRQMCore.Collections.Concurrent;
using RRQMCore.Data.Security;
using RRQMCore.Data.XML;
using RRQMCore.Diagnostics;
using RRQMCore.IO;
using RRQMCore.Pool;
using RRQMCore.Run;
using RRQMCore.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMCorePerformanceTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("1.测试内存池并发性能");
            Console.WriteLine("2.测试内存池延迟释放性能");
            Console.WriteLine("3.测试RRQM序列化和系统序列化");
            Console.WriteLine("4.测试等待池");
            Console.WriteLine("5.测试TestConcurrentList");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestBytePoolPerformance_one();
                        break;
                    }
                case "2":
                    {
                        TestBytePoolPerformance_two();
                        break;
                    }
                case "3":
                    {
                        TestSerializePerformance();
                        break;
                    }
                case "4":
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Task.Run(() =>
                            {
                                TestWaitPool();
                            });
                        }
                        break;
                    } 
                case "5":
                    {
                        TestConcurrentList();
                        break;
                    }
                default:
                    break;
            }

            Console.ReadKey();
        }

        static void TestConcurrentList()
        {
            ConcurrentList<int> list = new ConcurrentList<int>();
            Task.Run(()=> 
            {
                while (true)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        list.Add(i);
                    }
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        list.Remove(i);
                    }
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    int count = 0;
                    foreach (var item in list)
                    {
                        count++;
                    }

                    Console.WriteLine($"count={count}");
                }
            });
        }

        static WaitHandlePool<IWaitResult> WaitHandlePool = new WaitHandlePool<IWaitResult>();
        static void TestWaitPool()
        {
            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
             {
                 WaitResult waitResult = new WaitResult();
                 WaitData<IWaitResult> waitData = WaitHandlePool.GetWaitData(waitResult);

                 waitData.Wait(1000 * 5);
             });

            Console.WriteLine(timeSpan);
        }

        /// <summary>
        /// 获取文件的MD5码
        /// </summary>
        /// <param name="fileName">传入的文件名（含路径及后缀名）</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        private static void TestBytePoolPerformance_one()
        {
            TimeSpan time = TimeSpan.Zero;
            ThreadPool.SetMinThreads(10, 10);
            for (int j = 0; j < 10; j++)//10并发,总计1千万次
            {
                Task.Run(() =>
                {
                    TimeSpan timeSpan = TimeMeasurer.Run(() =>
                    {
                        for (int i = 0; i < 1000000; i++)//每次申请，销毁100w次
                        {
                            //byte[] buffer = new byte[1024*64];
                            ByteBlock byteBlock = new ByteBlock(1024 * 64);
                            byteBlock.Dispose();
                        }
                    });
                    time += timeSpan;
                    Console.WriteLine(timeSpan);
                    GC.Collect();
                });
            }

            Console.WriteLine("测试结束后，按任意键获取测试信息。");
            Console.ReadKey();
            Console.WriteLine($"内存池当前容量：{BytePool.GetPoolSize()}");
            Console.WriteLine($"总用时：{time}");
        }

        private static void TestBytePoolPerformance_two()
        {
            ThreadPool.SetMinThreads(100, 100);
            List<ByteBlock> byteBlocks = new List<ByteBlock>();

            bool run = true;
            Task.Run(() =>
            {
                while (run)
                {
                    int len = byteBlocks.Count;
                    lock (typeof(Program))
                    {
                        byteBlocks.RemoveRange(0, len);
                        Console.WriteLine(len);
                    }
                    Thread.Sleep(10);
                }
            });

            BytePool.AddSizeKey(1024 * 64);

            for (int n = 0; n < 10; n++)
            {
                Task.Run(async () =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            ByteBlock byteBlock = BytePool.GetByteBlock(1024 * 64 + j);
                            lock (typeof(Program))
                            {
                                byteBlocks.Add(byteBlock);
                            }
                        }

                        await Task.Delay(10);
                    }
                });
            }
            Console.ReadKey();
            GC.Collect();
            BytePool.Clear();
            run = false;
            Console.WriteLine($"内存池当前容量：{BytePool.GetPoolSize()}");
        }

        private static void TestSerializePerformance()
        {
            Student student = new Student();
            student.P1 = 10;
            student.P2 = "若汝棋茗";
            student.P3 = 100;
            student.P4 = 0;
            student.P5 = DateTime.Now;
            student.P6 = 10;
            student.P7 = new byte[1024 * 64];

            Random random = new Random();
            random.NextBytes(student.P7);

            student.List1 = new List<int>();
            student.List1.Add(1);
            student.List1.Add(2);
            student.List1.Add(3);

            student.List2 = new List<string>();
            student.List2.Add("1");
            student.List2.Add("2");
            student.List2.Add("3");

            student.List3 = new List<byte[]>();
            student.List3.Add(new byte[1024]);
            student.List3.Add(new byte[1024]);
            student.List3.Add(new byte[1024]);

            student.Dic1 = new Dictionary<int, int>();
            student.Dic1.Add(1, 1);
            student.Dic1.Add(2, 2);
            student.Dic1.Add(3, 3);

            student.Dic2 = new Dictionary<int, string>();
            student.Dic2.Add(1, "1");
            student.Dic2.Add(2, "2");
            student.Dic2.Add(3, "3");

            student.Dic3 = new Dictionary<string, string>();
            student.Dic3.Add("1", "1");
            student.Dic3.Add("2", "2");
            student.Dic3.Add("3", "3");

            student.Dic4 = new Dictionary<int, Arg>();
            student.Dic4.Add(1, new Arg(1));
            student.Dic4.Add(2, new Arg(2));
            student.Dic4.Add(3, new Arg(3));

            TimeSpan timeSpan1 = TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    ByteBlock byteBlock = BytePool.GetByteBlock(1024 * 100);
                    SerializeConvert.RRQMBinarySerialize(byteBlock, student);
                    Student student1 = SerializeConvert.RRQMBinaryDeserialize<Student>(byteBlock.Buffer, 0);
                    byteBlock.Dispose();
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
            });

            TimeSpan timeSpan2 = TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    ByteBlock byteBlock = BytePool.GetByteBlock(1024 * 100);
                    SerializeConvert.BinarySerialize(byteBlock, student);
                    byteBlock.Position = 0;
                    Student student1 = SerializeConvert.BinaryDeserialize<Student>(byteBlock);
                    byteBlock.Dispose();
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
            });

            Console.WriteLine($"RRQM:{timeSpan1}");
            Console.WriteLine($"System:{timeSpan2}");
        }

        private void TestSerialize()
        {
            string obj = "RRQM";

            //用系统二进制序列化、反序列化
            byte[] data1 = SerializeConvert.BinarySerialize(obj);
            string newObj = SerializeConvert.BinaryDeserialize<string>(data1);

            //用系统二进制序列化至文件、反序列化
            SerializeConvert.BinarySerializeToFile(obj, "C:/1.txt");
            string newFileObj = SerializeConvert.BinaryDeserializeFromFile<string>("C:/1.txt");

            //用RRQM二进制序列化、反序列化
            byte[] data2 = SerializeConvert.RRQMBinarySerialize(obj);
            string newRRQMObj = SerializeConvert.RRQMBinaryDeserialize<string>(data2, 0);

            //用Xml序列化、反序列化
            byte[] data3 = SerializeConvert.XmlSerializeToBytes(obj);
            string newXmlObj = SerializeConvert.XmlDeserializeFromBytes<string>(data3);
        }

        private void TestFileControler()
        {
            //判断该文件时候已在打开状态
            // FileControler.FileIsOpen("C:/1.txt");

            //获取文件SHA256值，转为大写16进制
            FileControler.GetFileHash("C:/1.txt");
        }

        private void TestTimeRun()
        {
            Action action = new Action(() => { Console.WriteLine("Hello"); });
            EasyAction.DelayRun(2000, action);//延迟两秒执行
            EasyAction.DelayRun(TimeSpan.FromSeconds(2), action);//延迟两秒执行
        }

        private void TestTimeMeasurer()
        {
            TimeSpan timeSpan = TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Console.WriteLine(i);
                }
            });

            Console.WriteLine(timeSpan);
        }

        private void TestXml()
        {
            XmlTool xmlTool = new XmlTool("Test.xml");

            //储存单节点、单属性值
            xmlTool.AttributeStorage("Node1", "AttributeName", "AttributeValue");

            //储存单节点、多属性值
            string[] attributeNames = new string[] { "A1", "A2" };
            string[] attributeValues = new string[] { "V1", "V2" };
            xmlTool.AttributeStorage("Node2", attributeNames, attributeValues);

            //储存多节点、多属性
            string[] nades = new string[] { "N1", "N2" };
            xmlTool.AttributeStorage(nades, attributeNames, attributeValues);

            //判断Node1节点是否存在
            xmlTool.NodeExist("Node1");

            //获取Node1节点下，属性名为AttributeName的属性值。
            string attributeValue = xmlTool.SearchWords("Node1", "AttributeName");

            //获取Node2下所有属性集合，并包装为字典
            Dictionary<string, string> attributes = xmlTool.SearchAllAttributes("Node2");
        }

        private void Test3DES()
        {
            DataLock.EncryptDES(new byte[10], "RRQM1234");
            byte[] data = DataLock.DecryptDES(new byte[10], "RRQM1234");
        }

        private void TestAppMessenger()
        {
            //注册静态方法
            AppMessenger.Default.Register(null, "SayHelloOne", MyMessage.SayHelloOne);

            //注册实例单个方法
            MyMessage myMessage = new MyMessage();
            AppMessenger.Default.Register(myMessage, "SayHelloTwo", myMessage.SayHelloTwo);

            //注册实例中被RegistMethod标记的公共方法。
            AppMessenger.Default.Register(new MyMessage());

            //触发已注册的SayHelloOne方法
            AppMessenger.Default.Send("SayHelloOne", null);

            //触发已注册的SayHelloThree方法，传入string参数，返回string参数。
            string mes = AppMessenger.Default.Send<string>("SayHelloThree", "若汝棋茗");
        }

        private static void CreatWaitHandle()
        {
            WaitHandlePool<MyWaitResult> waitHandle = new WaitHandlePool<MyWaitResult>();
            WaitData<MyWaitResult> waitData = waitHandle.GetWaitData(new MyWaitResult());
            waitData.Wait(10 * 1000);

            waitData.Set(new MyWaitResult());

            MyWaitResult myWaitResult = waitData.WaitResult;
        }

        private static void CreatObjectPool()
        {
            ObjectPool<MyObject> objectPool = new ObjectPool<MyObject>();
            MyObject myObject = objectPool.GetObject();
            objectPool.DestroyObject(myObject);
        }

        private static void TestBytePool()
        {
            TimeSpan timeSpan1 = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    byte[] buffer = new byte[1024 * 1024];
                }
            });

            TimeSpan timeSpan2 = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    ByteBlock byteBlock = BytePool.GetByteBlock(1024 * 1024);
                    byteBlock.Dispose();
                }
            });
            Console.WriteLine($"System:{timeSpan1}");
            Console.WriteLine($"RRQMCore:{timeSpan2}");
        }

        private static void CreatByteBlock()
        {
            //获取不小于64kb长度ByteBlock
            ByteBlock byteBlock1 = BytePool.GetByteBlock(1024 * 64);

            //获取64kb长度ByteBlock，且必须为64kb
            ByteBlock byteBlock2 = BytePool.GetByteBlock(1024 * 64, true);

            byteBlock1.Write(10);//写入byte
            byte[] buffer = new byte[1024];
            new Random().NextBytes(buffer);
            byteBlock1.Write(new byte[1024]);//写入byte[]

            byteBlock1.Dispose();
            byteBlock2.Dispose();//回收至内存池

            byteBlock1.SetHolding(true);
        }
    }

    public class MyMessage : IMessage
    {
        public static void SayHelloOne()
        {
        }

        public void SayHelloTwo()
        {
        }

        [AppMessage]
        public string SayHelloThree(string name)
        {
            return "SayHelloThree";
        }
    }

    internal class MyWaitResult : WaitResult
    {
    }

    internal class MyObject : IPoolObject
    {
        public bool NewCreate { get; set; }

        public void Create()
        {
        }

        public void Destroy()
        {
        }

        public void Recreate()
        {
        }
    }

    [Serializable]
    public class Student
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
        public long P3 { get; set; }
        public byte P4 { get; set; }
        public DateTime P5 { get; set; }
        public double P6 { get; set; }
        public byte[] P7 { get; set; }

        public List<int> List1 { get; set; }
        public List<string> List2 { get; set; }
        public List<byte[]> List3 { get; set; }

        public Dictionary<int, int> Dic1 { get; set; }
        public Dictionary<int, string> Dic2 { get; set; }
        public Dictionary<string, string> Dic3 { get; set; }
        public Dictionary<int, Arg> Dic4 { get; set; }
    }

    [Serializable]
    public class Arg
    {
        public Arg()
        {
        }

        public Arg(int myProperty)
        {
            this.MyProperty = myProperty;
        }

        public int MyProperty { get; set; }
    }

    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}