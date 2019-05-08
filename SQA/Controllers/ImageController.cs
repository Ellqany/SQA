using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQA.Abstraction;
using System.IO;
using System.Threading.Tasks;

namespace SQA.Controllers
{
    public class ImageController : Controller
    {
        #region Private Variables
        readonly IImageRepository ImageRepository;
        #endregion

        #region Constractor
        public ImageController(IImageRepository imageRepository)
        {
            ImageRepository = imageRepository;
        }
        #endregion

        [HttpGet]
        public IActionResult Index() => View(ImageRepository.GetAll());
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile Image)
        {
            if (Image != null)
            {
                await ImageRepository.AddImage(Image);
                return RedirectToAction(nameof(ImageController.Index));
            }
            ModelState.AddModelError("Image", "Please Select an Image");
            return View(ImageRepository.GetAll());
        }
        [HttpGet]
        public FileStreamResult ViewImage(string uname)
        {
            var Image = ImageRepository.GetById(uname);
            MemoryStream ms = new MemoryStream(Image.ImageData);
            return new FileStreamResult(ms, Image.ImageExtention);
        }

        #region Private Variables

        #endregion

    }
}