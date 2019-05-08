using Microsoft.AspNetCore.Identity;
using SQA.Models;
using SQA.Models.Identity;
using SQA.Services.Abstraction;
using System.Threading.Tasks;

namespace SQA.Services.Concreate
{
    public class UserServices : IUserServices
    {
        #region Private Variables
        readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole> RoleManager;
        #endregion

        #region Constractor
        public UserServices(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            RoleManager = roleManager;
        }
        #endregion

        public async Task<IdentityResult> Create(CreateUser CreateUser)
        {
            User user = new User
            {
                Name = CreateUser.Name,
                UserName = CreateUser.UserName,
                Gender = CreateUser.Gender,
                Email = CreateUser.Email,
                PhoneNumber = CreateUser.Phone,
                Department = CreateUser.Department
            };
            if (await RoleManager.FindByNameAsync("Student") == null)
            {
                await RoleManager.CreateAsync(new IdentityRole("Student"));
            }
            return await _userManager.CreateAsync(user, CreateUser.Password);
        }

        public async Task<IdentityResult> Edit(EditUser EditUser)
        {
            User user = await _userManager.FindByIdAsync(EditUser.Id);
            user.Name = EditUser.Name;
            user.UserName = EditUser.UserName;
            user.Gender = EditUser.Gender;
            user.Email = EditUser.Email;
            user.PhoneNumber = EditUser.Phone;
            user.Department = EditUser.Department;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePassword(ChangePassword ChangePassword)
        {
            var User = await _userManager.FindByNameAsync
                (ChangePassword.UserName);
            return await _userManager.ChangePasswordAsync
                (User, ChangePassword.OldPassword, ChangePassword.Password);
        }

    }
}
