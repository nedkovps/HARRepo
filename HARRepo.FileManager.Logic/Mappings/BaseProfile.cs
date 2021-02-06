using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Mappings
{
    public class BaseProfile : AutoMapper.Profile
    {
        public BaseProfile()
        {
            CreateMap<BaseEntity, BaseDTO>().ReverseMap();
        }
    }
}
