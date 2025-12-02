using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create your mappings here
            CreateMap<TrainerProfile, TrainerProfileResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));
        }
    }
}
