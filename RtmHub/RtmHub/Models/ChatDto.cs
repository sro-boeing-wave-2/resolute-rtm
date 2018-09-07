using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RtmHub.Models
{
    public class ChatDto
    {
        string description;
        int userid;
        string customerhandle;

        public string Description { get => description; set => description = value; }
        public string Customerhandle { get => customerhandle; set => customerhandle = value; }
        public int Userid { get => userid; set => userid = value; }
    }
}
