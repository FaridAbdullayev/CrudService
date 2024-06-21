using Core.Entities;
using Microsoft.AspNetCore.Http;
using Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Service.Profiles
{
    public class MapProfile:Profile
    {
        private readonly IHttpContextAccessor _context;

        public MapProfile(IHttpContextAccessor httpContextAccessor)
        {

            _context = httpContextAccessor;

            var uriBuilder = new UriBuilder(_context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host.Host, _context.HttpContext.Request.Host.Port ?? -1);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }
            string baseUrl = uriBuilder.Uri.AbsoluteUri;

            CreateMap<Group, GroupGetDto>();
            //.ForMember(dest => dest.Name, s => s.MapFrom(s => s.No));
            CreateMap<GroupCreateDto, Group>();
            CreateMap<Group, GroupListItemGetDto>();

            CreateMap<Student, StudentGetDto>()
                .ForMember(dest => dest.Age, s => s.MapFrom(s => DateTime.Now.Year - s.BirthDay.Year))
                .ForMember(dest => dest.ImageUrl, s => s.MapFrom(s => baseUrl + "image/" + s.Image));
        }
    }
}
