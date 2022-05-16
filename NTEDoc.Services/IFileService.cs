using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NTEDoc.Services
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file);
        void DeleteFile(string path);
    }
}
