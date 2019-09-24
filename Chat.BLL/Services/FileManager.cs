using Chat.BLL.Interfaces;
using Chat.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using File = Chat.Models.Entities.File;

namespace Chat.BLL.Services
{
   public class FileManager:IFileManager
    {
        public IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment appEnvironment;
        public FileManager(IUnitOfWork _unitOfWork, IHostingEnvironment _appEnvironment)
        {
            unitOfWork = _unitOfWork;
            appEnvironment = _appEnvironment;
        }
        public string SaveImage(IFormFile image)
        {
            var folderName = Path.Combine("Resources", "Images");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (image.Length > 0)
            {
                var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";

                //var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                return dbPath;
            }
            else
                return null;
        }
        public async Task<ICollection<File>> UploadMessagesFiles(int dialogId, int messageId, IFormFileCollection files)
        {
            var folderName = Path.Combine("Resources", "Dialogs");
            var directory = Path.Combine(Directory.GetCurrentDirectory(), folderName +"\\"+ dialogId + "\\");
            //string directory = Path.Combine(appEnvironment.WebRootPath + "\\Dialogs\\" + dialogId + "\\");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileCollection = new List<File>();
            foreach (var file in files)
            {
                
                string fileName =   file.FileName;
                var dbPath = Path.Combine(folderName+ "\\"+dialogId, fileName);
                using (var fileStream = new FileStream(directory + file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                File newfile = new File { Name = file.FileName, Path = dbPath, MessageId = messageId };
                await unitOfWork.Files.Create(newfile);
                await unitOfWork.Files.SaveAsync();
                fileCollection.Add(newfile);
            }
            return fileCollection;
        }
    }
}
