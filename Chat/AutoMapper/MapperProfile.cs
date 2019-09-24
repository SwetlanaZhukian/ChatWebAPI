using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.BLL.DTO;
using Chat.Models.Entities;
using Chat.ViewModels;

namespace Chat.AutoMapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<User, ProfileViewModel>().ReverseMap();

            CreateMap<User, EditViewModel>().ReverseMap();

            CreateMap<User, ContactViewModel>(). ReverseMap();

            CreateMap<Contact, ContactViewModel>()
           .ForMember(c => c.UserId, s => s.MapFrom(x => x.Friend.Id))
           .ForMember(c =>c.Name, s => s.MapFrom(x => x.Friend.Name))     
           .ForMember(c => c.Email, s => s.MapFrom(x => x.Friend.Email))
           .ForMember(c => c.PhoneNumber, s => s.MapFrom(x => x.Friend.PhoneNumber))
           .ForMember(c => c.Avatar, s => s.MapFrom(x => x.Friend.Avatar))  
           .ReverseMap();

            CreateMap<BlackList, ContactViewModel>()
           .ForMember(c => c.UserId, s => s.MapFrom(x => x.ContactBlock.Id))
           .ForMember(c => c.Name, s => s.MapFrom(x => x.ContactBlock.Name))
           .ForMember(c => c.Email, s => s.MapFrom(x => x.ContactBlock.Email))
           .ForMember(c => c.PhoneNumber, s => s.MapFrom(x => x.ContactBlock.PhoneNumber))
           .ForMember(c => c.Avatar, s => s.MapFrom(x => x.ContactBlock.Avatar))
           .ReverseMap();

           

        }
    }
}
