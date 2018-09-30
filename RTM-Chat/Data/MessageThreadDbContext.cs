using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using RTM_Chat.Models;
using Microsoft.Extensions.Options;

namespace RTM_Chat.Data
{
    public class MessageThreadDbContext
    {
        MongoClient _client;
        IMongoDatabase _db;

        public MessageThreadDbContext(IOptions<Settings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _db = _client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<MessageThread> MessageThreadCollection 
        {
            get 
            {
                return _db.GetCollection<MessageThread>("MessageThread");
            }
        }
    }
}
