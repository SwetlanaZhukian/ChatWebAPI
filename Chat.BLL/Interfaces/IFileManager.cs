using Chat.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using File = Chat.Models.Entities.File;

namespace Chat.BLL.Interfaces
{
    public interface IFileManager
    {
        string SaveImage(IFormFile image);
        Task<ICollection<File>> UploadMessagesFiles(int dialogId, int messageId, IFormFileCollection files);

    }
}
