using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTM_Chat.IO
{
    public class Constants
    {
        public static string BASE_URL = "http://" + Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
        public static string ENDUSER_URL = "/endusers/query?Email=";
        public static string TICKET_URL = "/Tickets";
       
    }
}
