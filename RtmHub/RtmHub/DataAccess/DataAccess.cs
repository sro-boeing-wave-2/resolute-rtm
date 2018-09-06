using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RtmHub.Models;

namespace RtmHub.DataAccess
{
    public class DataAccess
    {
        MongoClient _client;
        IMongoDatabase _db;

        public DataAccess()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _db = _client.GetDatabase("temp2");
        }
        
        public MongoTicket GetTicket(int ticketid)
        {
            return _db.GetCollection<MongoTicket>("Ticket").AsQueryable().SingleOrDefault(x => x.TicketId == ticketid);
        }

        public void PostTicket(MongoTicket ticket)
        {
            _db.GetCollection<MongoTicket>("Ticket").InsertOneAsync(ticket);
        }

        public void PutTicket(MongoTicket ticket)
        {
            var sid = new ObjectId(ticket.Id.ToString());

            var filter = Builders<MongoTicket>.Filter.Eq("Id", sid);
            var update = Builders<MongoTicket>.Update
                        .Set(s => s.TicketId , ticket.TicketId)
                        .Set(s => s.Conversations , ticket.Conversations);
            _db.GetCollection<MongoTicket>("Ticket").UpdateOne(filter, update);
        }

    }
}
