using RTM_Chat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTM_Chat.Services
{
    public interface IMessageThreadService
    {
        MessageThread GetMessageThreadByTicketId(string ticketId);
        MessageThread CreateMessageThread(MessageThread messageThread);

        List<Message> GetListMessageThread(string threadid);
    }
}