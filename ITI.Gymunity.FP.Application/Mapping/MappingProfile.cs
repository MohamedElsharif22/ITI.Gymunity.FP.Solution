using AutoMapper;
using ITI.Gymunity.FP.Infrastructure.DTOs.Trainer;
using ITI.Gymunity.FP.Infrastructure.DTOs.Messaging;
using ITI.Gymunity.FP.Infrastructure.DTOs.Notifications;
using ITI.Gymunity.FP.Infrastructure.DTOs.User.Payment;
using ITI.Gymunity.FP.Infrastructure.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models.Messaging;

namespace ITI.Gymunity.FP.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
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

            CreateMap<Subscription, SubscriptionResponse>()
                .ForMember(d => d.PackageName,
                    o => o.MapFrom(s => s.Package.Name))
                .ForMember(d => d.PackageDescription,
                    o => o.MapFrom(s => s.Package.Description ?? ""))
                // Trainer Info
                .ForMember(d => d.TrainerId,
                    o => o.MapFrom(s => s.Package.TrainerId))
                .ForMember(d => d.TrainerName,
                    o => o.MapFrom(s => s.Package.Trainer.FullName))
                .ForMember(d => d.TrainerHandle,
                    o => o.MapFrom(s => s.Package.Trainer.UserName))
                .ForMember(d => d.TrainerPhotoUrl,
                    o => o.MapFrom(s => s.Package.Trainer.ProfilePhotoUrl))
                .ForMember(d => d.IsAnnual,
                    o => o.MapFrom(s => (s.CurrentPeriodEnd - s.StartDate).Days > 31));
                //.ForMember(d => d.FeaturesIncluded,
                //    o => o.MapFrom(s => ParseFeatures(s.Package.FeaturesJSON)));


            // Message mappings
            CreateMap<Message, MessageResponse>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(m => m.Sender.FullName))
                .ForMember(dest => dest.SenderProfilePhoto, opt => opt.MapFrom(m => m.Sender.ProfilePhotoUrl));

            // Notification mappings
            CreateMap<Notification, NotificationResponse>();

            CreateMap<Payment, PaymentResponse>()
               .ForMember(d => d.TransactionId,
                   o => o.MapFrom(s => s.PaymobTransactionId ?? s.PayPalOrderId))
               .ForMember(d => d.PaymentUrl,
                   o => o.Ignore());
        }

    }
}