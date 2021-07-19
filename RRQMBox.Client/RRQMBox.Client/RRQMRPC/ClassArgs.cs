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

}
