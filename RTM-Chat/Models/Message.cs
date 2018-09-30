using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RTM_Chat.Models
{
    public class Message
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("EmailId")]
        public string EmailId { get; set; }
        [BsonElement("Message")]
        public string MessageText { get; set; }
        [BsonElement("TimeStamp")]
        public DateTime Timestamp { get; set; }
    }
}

