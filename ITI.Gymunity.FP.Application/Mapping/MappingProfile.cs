using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Domain.Models.Client;
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

            CreateMap<ClientProfile, ClientProfileResponse>();
            //.ForMember(dest => dest.UserName, opt => opt.MapFrom(cp => cp.User.UserName))
            //.ForMember(dest => dest.FullName, opt => opt.MapFrom(cp => cp.User.FullName))
            //.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(cp => cp.User.PhoneNumber))
            //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(cp => cp.CreatedAt))
            //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(cp => cp.UpdatedAt));

            CreateMap<ClientProfileRequest, ClientProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
                //.ForMember(dest => dest.User, opt => opt.Ignore())
                //.ForMember(dest => dest.Subscriptions, opt => opt.Ignore());
        }
    }
}
