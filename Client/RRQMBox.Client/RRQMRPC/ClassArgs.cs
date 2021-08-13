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
using System;
using RRQMSocket.RPC;
using RRQMCore.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMRPC.RRQMTest
{

public class Test02
{
public System.Int32 Age{get;set;}
public System.String Name{get;set;}
public List<System.Int32> list{get;set;}
public System.Int32[] nums{get;set;}
}


public class Test01
{
public System.Int32 Age{get;set;}
public System.String Name{get;set;}
}


public class FileModel
{
public System.Int32 Id{get;set;}
public System.String FileName{get;set;}
public System.String UpdateDate{get;set;}
public System.Int64 Size{get;set;}
public System.String Remarks{get;set;}
public System.String FilePath{get;set;}
public System.String ServerWorkspace{get;set;}
}


public class ProxyClass3
{
public System.Int32 P1{get;set;}
}


public class ProxyClass2
{
public System.Int32 P1{get;set;}
public ProxyClass3 P2{get;set;}
}


public class ProxyClass1
{
public System.Int32 P1{get;set;}
public ProxyClass2 P2{get;set;}
}

}
