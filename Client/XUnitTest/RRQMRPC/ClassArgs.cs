using System;
using RRQMSocket.RPC;
using RRQMCore.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMRPC.RRQMTest
{

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


public class Class01
{
public System.Int32 Age{get;set;}
public System.String Name{get;set;}
}


public class Args
{
public System.Int32 P1{get;set;}
public System.Double P2{get;set;}
public System.String P3{get;set;}
}


public class Class04
{
public System.Int32 P1{get;set;}
public System.String P2{get;set;}
public System.Int32 P3{get;set;}
}


public struct StructArgs
{
public System.Int32 P1{get;set;}
}

}
