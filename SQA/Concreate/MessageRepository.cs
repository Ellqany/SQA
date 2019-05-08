using Microsoft.AspNetCore.Identity;
using SQA.Abstraction;
using SQA.Models;
using SQA.Models.FacultyContext;
using SQA.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Concreate
{
    public class MessageRepository : IMessageRepository
    {
        #region Private Variables
        readonly IGenericServices GenericServices;
        readonly FacultyDBContext facultyDBContext;
        readonly UserManager<User> UserManager;
        #endregion

        #region Constractor
        public MessageRepository(
            IGenericServices genericServices,
            UserManager<User> userManager,
            FacultyDBContext _facultyDBContext)
        {
            GenericServices = genericServices;
            facultyDBContext = _facultyDBContext;
            UserManager = userManager;
        }
        #endregion

        public async Task<List<Message>> AddMessage(User user, CreateMessage _message)
        {
            if (string.IsNullOrEmpty(_message.txtMessage))
            {
                var _messages = await GetMessages(user, _message.UserName);
                return _messages.Messages;
            }
            else
            {
                await facultyDBContext.Messages.AddAsync(new Message
                {
                    MessageID = await GenericServices.GetIDAsyc(),
                    Content = _message.txtMessage,
                    Date = DateTime.Now,
                    Object = user.UserName,
                    Subject = _message.UserName
                });
                await facultyDBContext.SaveChangesAsync();
                var _messages = await GetMessages(user, _message.UserName);
                return _messages.Messages;
            }
        }

        public async Task<CreateMessage> GetMessages(User user, string UserName)
        {
            string name = user.UserName;
            return await GetMessages(name, UserName);
        }

        public async Task<CreateMessage> GetMessages(string Sender, string UserName)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var messages = facultyDBContext.Messages.
                    Where(x => x.Subject == UserName && x.Object == Sender).ToList();
                var message2 = facultyDBContext.Messages.
                    Where(x => x.Object == UserName && x.Subject == Sender).ToList();
                messages.AddRange(message2);
                return new CreateMessage
                {
                    Messages = messages.Distinct().OrderBy(x => x.Date).ToList(),
                    Sender = Sender,
                    UserName = UserName
                };
            });
            await task;
            return task.Result;
        }

        public async Task<User> GetUserId(string UserName) =>
            await UserManager.FindByNameAsync(UserName);

    }
}
