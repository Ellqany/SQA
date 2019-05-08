using SQA.Models;
using SQA.Models.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQA.Abstraction
{
    public interface IMessageRepository
    {
        Task<List<Message>> AddMessage(User user, CreateMessage _message);

        Task<User> GetUserId(string UserName);

        Task<CreateMessage> GetMessages(User user, string UserName);

        Task<CreateMessage> GetMessages(string Sender, string UserName);

    }
}
