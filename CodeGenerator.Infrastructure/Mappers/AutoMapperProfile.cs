﻿using AutoMapper;
using CodeGenerator.Application.Responses;
using CodeGenerator.Infrastructure.Context.Models;

namespace CodeGenerator.Infrastructure.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Table, TableResponse>();
        }
    }
}
