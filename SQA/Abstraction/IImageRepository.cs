using Microsoft.AspNetCore.Http;
using SQA.Models.FacultyContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQA.Abstraction
{
    public interface IImageRepository
    {
        #region Get Methods
        Image GetById(string Id);

        IEnumerable<Image> GetAll();
        #endregion

        #region Operation Methods
        Task<int> AddImage(IFormFile file);
        #endregion
    }
}
