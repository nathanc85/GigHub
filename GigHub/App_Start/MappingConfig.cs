using AutoMapper;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.App_Start
{
    public static class MappingConfig
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(
                cfg => {
                    cfg.CreateMap<ApplicationUser, UserDto>();
                    cfg.CreateMap<Gig, GigDto>();
                    cfg.CreateMap<Notification, NotificationDto>();
                }
            );

            return config.CreateMapper();
        }
    }
}