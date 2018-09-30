using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using RTM_Chat.Models;

namespace RTM_Chat.Hubs
{
    public class ClientHandler
    {
        public static Dictionary<string,MessageThread> clienthandler = new Dictionary<string, MessageThread>();
    }
}
