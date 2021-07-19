using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMBox.Server
{
    public class ServerTwo : ServerProvider
    {
        [RRQMRPC]
        public void AreYouOk(int a)
        {

        }

        [RRQMRPC]
        public Student UpdateStudent(Student student)
        {
            student.Name = "RRQM";
            return student;
        }
    }

    public class Student
    {
        public string Name { get; set; }
    }
}
