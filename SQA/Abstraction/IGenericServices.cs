using Microsoft.AspNetCore.Http;
using SQA.Models.FacultyContext;
using System.Threading.Tasks;

namespace SQA.Abstraction
{
    public interface IGenericServices
    {
        Task<Image> GetFileAsync(IFormFile formFile);
        Task<string> GetIDAsyc();
    }
}
