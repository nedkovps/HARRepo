using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Mappings
{
    public class DirectoryProfile : AutoMapper.Profile
    {
        public DirectoryProfile()
        {
            CreateMap<Directory, DirectoryDTO>()
                .IncludeBase<BaseEntity, BaseDTO>();
        }
    }
}
