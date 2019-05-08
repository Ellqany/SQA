using Microsoft.AspNetCore.Http;
using SQA.Abstraction;
using SQA.Models.FacultyContext;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SQA.Concreate
{
    public class GenericServices : IGenericServices
    {
        public async Task<Image> GetFileAsync(IFormFile formFile)
        {
            MemoryStream ms = new MemoryStream();
            await formFile.OpenReadStream().CopyToAsync(ms);
            return new Image
            {
                ImageData = ms.ToArray(),
                ImageExtention = formFile.ContentType,
            };
        }

        public async Task<string> GetIDAsyc()
        {
            var task = Task.Factory.StartNew(() =>
            {
                return GetID();
            });
            await task;
            return task.Result;
        }

        public string GetID()
        {
            var guid = Guid.NewGuid();
            return guid.ToString() + DateTime.Now.ToLongDateString();
        }
    }
}
