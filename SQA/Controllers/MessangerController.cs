using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQA.Abstraction;
using SQA.Models;
using SQA.Models.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Controllers
{
    [Authorize]
    public class MessangerController : Controller
    {
        #region Private Variables
        readonly IMessageRepository MessageRepository;
        readonly UserManager<User> _userManager;
        Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constractor
        public MessangerController(
            UserManager<User> userManager,
            IMessageRepository messageRepository)
        {
            MessageRepository = messageRepository;
            _userManager = userManager;
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var user = await CurrentUser;
            return View(_userManager.Users
            .Where(x => x.UserName != user.UserName)
            .OrderBy(x => x.UserName).ToList());
        }

        public async Task<IActionResult> GetMessage(string UserName) =>
            View(await MessageRepository.GetMessages(await CurrentUser, UserName));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateMessage _message)
            => View("Messages",
                await MessageRepository.AddMessage(await CurrentUser, _message));
    }
}