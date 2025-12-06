using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain.Models.Trainer;
<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> main

namespace ITI.Gymunity.FP.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
<<<<<<< HEAD
            // TrainerProfile to List Response (excludes status fields)
            CreateMap<TrainerProfile, TrainerProfileListResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));

            // TrainerProfile to Detail Response (includes status fields)
            CreateMap<TrainerProfile, TrainerProfileDetailResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));

            // Create request to TrainerProfile
            CreateMap<CreateTrainerProfileRequest, TrainerProfile>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDescription, opt => opt.Ignore());

            // Update request to TrainerProfile (only maps non-null values)
            CreateMap<UpdateTrainerProfileRequest, TrainerProfile>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDescription, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
=======
            // Create your mappings here
            CreateMap<TrainerProfile, TrainerProfileResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));
>>>>>>> main
        }
    }
}