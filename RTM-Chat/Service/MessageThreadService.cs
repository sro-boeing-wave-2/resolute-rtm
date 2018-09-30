using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RTM_Chat.Models;
using RTM_Chat.Data;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;

namespace RTM_Chat.Services
{
    class MessageThreadService: IMessageThreadService
    {
        public MessageThreadDbContext _context;
        public MessageThreadService(IOptions<Settings> settings)
        {
            _context = new MessageThreadDbContext(settings);
        }

        public MessageThread GetMessageThreadByTicketId(string ticketId)
        {
            var filter = Builders<MessageThread>.Filter.Eq("TicketId", ticketId);
            var messageThread = _context.MessageThreadCollection.Find(filter).FirstOrDefault();
            return messageThread;
        }

        public MessageThread CreateMessageThread(MessageThread messageThread)
        {
            _context.MessageThreadCollection.InsertOne(messageThread);
            return messageThread;
        }
    }
}