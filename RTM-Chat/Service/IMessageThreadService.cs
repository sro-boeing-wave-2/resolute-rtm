using RTM_Chat.Models;

namespace RTM_Chat.Services
{
    public interface IMessageThreadService
    {
        MessageThread GetMessageThreadByTicketId(string ticketId);
        MessageThread CreateMessageThread(MessageThread messageThread);
    }
}