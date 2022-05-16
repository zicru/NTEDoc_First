using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace NTEDoc.Services
{
    public class FileService : IFileService

    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public FileService(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<IEnumerable<string>> SaveMultipleFiles(IFormFileCollection files)
        {
            var output = new List<string>();
            
            foreach (var file in files)
            {
                var savedFile = await SaveFile(file);

                output.Add(savedFile);
            }

            return output;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var uniqueName = Guid.NewGuid().ToString();
            var extension = file.FileName.Split(".")[1].ToLower();
            var fullName = uniqueName + "." + extension;
            string path = Path.Combine(_configuration["FilePath:FolderLocation"], fullName);

            try {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
            } catch(Exception e) {
                System.Console.WriteLine(e);
            }

            return fullName;
        }

        public void DeleteFile(string path)
        {
            //var currentFile = Path.Combine(_hostingEnvironment.WebRootPath, path.Substring(1));
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
