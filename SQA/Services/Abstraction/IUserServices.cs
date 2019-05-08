using Microsoft.AspNetCore.Identity;
using SQA.Models;
using System.Threading.Tasks;

namespace SQA.Services.Abstraction
{
    public interface IUserServices
    {
        Task<IdentityResult> Create(CreateUser CreateUser);

        Task<IdentityResult> Edit(EditUser EditUser);

        Task<IdentityResult> ChangePassword(ChangePassword ChangePassword);
    }
}
