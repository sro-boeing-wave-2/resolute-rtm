using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RtmHub.Models
{
    public class Conversation
    {
        [BsonElement("To")]
        public Client To { get; set; }
        [BsonElement("From")]
        public Client From { get; set; }
        [BsonElement("Text")]
        public string Text { get; set; }
        [BsonElement("TimeStamp")]
        public string TimeStamp { get; set; }
    }
}