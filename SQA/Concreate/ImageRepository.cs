using Microsoft.AspNetCore.Http;
using SQA.Abstraction;
using SQA.Models.FacultyContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQA.Concreate
{
    public class ImageRepository : IImageRepository
    {
        #region Private Variables
        readonly IGenericServices GenericServices;
        readonly FacultyDBContext facultyDBContext;
        #endregion

        #region Constractor
        public ImageRepository(
            IGenericServices genericServices,
            FacultyDBContext _facultyDBContext)
        {
            GenericServices = genericServices;
            facultyDBContext = _facultyDBContext;
        }
        #endregion

        #region Get Methods
        public Image GetById(string Id)
        {
            return facultyDBContext.Images.SingleOrDefault(x => x.Id == Id);
        }

        public IEnumerable<Image> GetAll()
        {
            return facultyDBContext.Images;
        }
        #endregion

        #region Operation Methods
        public async Task<int> AddImage(IFormFile file)
        {
            var Image = await GenericServices.GetFileAsync(file);
            Image.Id = await GenericServices.GetIDAsyc();
            await facultyDBContext.Images.AddAsync(Image);
            return await facultyDBContext.SaveChangesAsync();
        }
        #endregion

    }
}
