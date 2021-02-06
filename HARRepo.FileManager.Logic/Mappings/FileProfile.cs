using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Mappings
{
    public class FileProfile : AutoMapper.Profile
    {
        public FileProfile()
        {
            CreateMap<File, FileDTO>()
                .IncludeBase<BaseEntity, BaseDTO>();
        }
    }
}
