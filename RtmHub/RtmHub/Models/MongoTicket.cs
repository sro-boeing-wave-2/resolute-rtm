using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RtmHub.Models
{
    public class MongoTicket
    {
        public ObjectId Id { get; set; }
        [BsonElement("TicketId")]
        public int TicketId { get; set; }
        [BsonElement("Conversations")]
        public List<Conversation> Conversations { get; set; }
    }
}
