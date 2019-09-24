using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Chat.BLL.DTO;
using Chat.Models.Entities;

namespace Chat.BLL.Automapper
{
   public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
