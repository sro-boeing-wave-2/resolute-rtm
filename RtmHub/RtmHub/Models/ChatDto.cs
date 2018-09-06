using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RtmHub.Models
{
    public class ChatDto
    {
        string description;
        string userhandle;
        string customerhandle;

        public string Description { get => description; set => description = value; }
        public string Userhandle { get => userhandle; set => userhandle = value; }
        public string Customerhandle { get => customerhandle; set => customerhandle = value; }
    }
}
