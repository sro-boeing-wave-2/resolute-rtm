using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RTM_Chat.Models
{
    public class MessageThread
    {   
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("TicketId")]
        public string TicketId { get; set; }

        [BsonElement("MessageDetail")]
        public List<Message> MessageDetails { get; set; }
    }
}
