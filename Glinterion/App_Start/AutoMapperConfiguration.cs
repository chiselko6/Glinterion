using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Glinterion.Models;
using Glinterion.ViewModels;

namespace Glinterion.App_Start
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<RegisterViewModel, User>();
        }
    }
}