using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Mappings
{
    public class SharedFileProfile : AutoMapper.Profile
    {
        public SharedFileProfile()
        {
            CreateMap<SharedFile, SharedFileDTO>()
                .IncludeBase<BaseEntity, BaseDTO>();

            CreateMap<SharedFile, SharedWithMeFileDTO>()
                .IncludeBase<BaseEntity, BaseDTO>()
                .ForMember(x => x.SharedById, y => y.MapFrom(z => z.OwnerId))
                .ForMember(x => x.SharedBy, y => y.MapFrom(z => z.Owner));
        }
    }
}
