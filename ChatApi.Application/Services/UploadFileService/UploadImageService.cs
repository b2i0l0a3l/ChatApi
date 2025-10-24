using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services.UploadFileService
{
    public class UploadImageService : IUploadFile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UploadImageService(IHttpContextAccessor httpContextAccessor)
           => _httpContextAccessor = httpContextAccessor;
        private string GeneratePhotoUrl(string picture)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            picture = $"{request?.Scheme}://{request?.Host}/Uploads/{picture}";
            return picture;
        }
        public async Task<(string,string)> UploadFile(IFormFile file)
        {
            var UploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(UploadFolder))
                Directory.CreateDirectory(UploadFolder);

            var FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(UploadFolder, FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return (GeneratePhotoUrl(FileName),filePath);
        }
    }
}
