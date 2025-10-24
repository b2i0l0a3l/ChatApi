using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Interfaces
{
    public interface IUploadFile
    {
        Task<(string,string)> UploadFile(IFormFile file);

    }
}