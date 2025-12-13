using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Domain.Models;
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
            CreateMap<TrainerProfile, TrainerProfileResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));

            CreateMap<CreateTrainerProfileRequest, TrainerProfile>();
            CreateMap<Subscription, SubscriptionResponse>()
    .ForMember(d => d.PackageName,
        o => o.MapFrom(s => s.Package.Name));

        }

    }
}
