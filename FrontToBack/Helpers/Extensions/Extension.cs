using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FrontToBack.Helpers.Extensions
{
    public static class Extension
    {
        public static bool CheckFile(this IFormFile file, string format)
        {
            return file.ContentType.Contains(format);
        }
        
        public static bool CheckFileLength(this IFormFile file, int size)
        {
            return file.Length/1024 > size;
        }

        public static void SaveFile(this IFormFile file, IWebHostEnvironment env, string folder)
        {
            string fileName = file.FileName;
            string path = Path.Combine(env.WebRootPath, folder, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        public static void DeleteFile(this string fileName, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder, fileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        
    }
}