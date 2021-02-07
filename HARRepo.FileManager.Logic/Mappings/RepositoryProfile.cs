using HARRepo.FileManager.Data.Entities;
using HARRepo.FileManager.Logic.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Mappings
{
    public class RepositoryProfile : AutoMapper.Profile
    {
        public RepositoryProfile()
        {
            CreateMap<Repository, RepositoryDTO>()
                .IncludeBase<BaseEntity, BaseDTO>();

            CreateMap<Repository, RepositoryDetailsDTO>()
                .IncludeBase<Repository, RepositoryDTO>();
        }
    }
}
