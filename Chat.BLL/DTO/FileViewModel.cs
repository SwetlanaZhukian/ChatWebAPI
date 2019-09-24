using Chat.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.BLL.DTO
{
   public class FileViewModel
    {
        public string FilesPath { get; set; }
        public string Extension { get; set; }

        public FileViewModel(File file)
        {
            Extension = System.IO.Path.GetExtension(file.Path);
            FilesPath = file.Path;
        }
    }
}
