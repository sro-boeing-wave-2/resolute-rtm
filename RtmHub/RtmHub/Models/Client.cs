using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RtmHub.Models
{
    public class Client
    {
        string clientemail;
        string clientname;
        string clientimageurl;
        string connectionid;
        string designation;
        string query;

        
        public string Connectionid { get => connectionid; set => connectionid = value; }
        public string Designation { get => designation; set => designation = value; }
        public string Query { get => query; set => query = value; }
        public string Clientemail { get => clientemail; set => clientemail = value; }
        public string Clientname { get => clientname; set => clientname = value; }
        public string Clientimageurl { get => clientimageurl; set => clientimageurl = value; }
    }
}
