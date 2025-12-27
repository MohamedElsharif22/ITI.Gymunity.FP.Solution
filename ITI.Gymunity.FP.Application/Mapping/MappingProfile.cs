using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.DTOs.Messaging;
using ITI.Gymunity.FP.Application.DTOs.Notifications;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Application.DTOs.User.Payment;
using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Models.Client;
using ITI.Gymunity.FP.Application.DTOs.Client;
using ITI.Gymunity.FP.Application.DTOs.ExerciseLibrary;
using ITI.Gymunity.FP.Application.DTOs.Package;
using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.Guest;
using ITI.Gymunity.FP.Application.DTOs.Admin;
using ITI.Gymunity.FP.Application.DTOs.ClientDto;
using ITI.Gymunity.FP.Application.Mapping.Resolvers;
using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using ITI.Gymunity.FP.Application.DTOs.User;

namespace ITI.Gymunity.FP.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //*********************     Admin Mapping       ******************************//
            CreateMap<AppUser, TrainerResponse>()
                .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.MapFrom<GenericImageUrlResolver<AppUser, TrainerResponse>, string?>(u => u.ProfilePhotoUrl));
            CreateMap<AppUser, ClientResponse>()
                .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.MapFrom<GenericImageUrlResolver<AppUser, ClientResponse>, string?>(u => u.ProfilePhotoUrl));

            //*********************    End of Admin Mapping       ******************************//
            //*********************     Client Profile Mapping       ******************************//
            CreateMap<ClientProfileRequest, ClientProfile>();
            CreateMap<ClientProfile, ClientProfileResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(cp => cp.User.UserName))
                .ForMember(dest => dest.BodyStateLog, opt => opt.MapFrom(src => src.BodyStatLogs
                .OrderByDescending(b => b.LoggedAt).FirstOrDefault()));

            //*********************     BodyStateLog Mapping       ******************************//
            CreateMap<CreateBodyStateLogRequest, BodyStatLog>();
            CreateMap<BodyStatLog, BodyStateLogResponse>();

            //*********************     BodyStateLog Mapping       ******************************//
            CreateMap<WorkoutLogRequest, WorkoutLog>();
            CreateMap<WorkoutLog, WorkoutLogResponse>();


            // TrainerProfile to List Response (excludes status fields)
            CreateMap<TrainerProfile, TrainerProfileListResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileListResponse>, string?>(tp => tp.CoverImageUrl))
                .ForMember(dest => dest.VideoIntroUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileListResponse>, string?>(tp => tp.VideoIntroUrl));

            // TrainerProfile to Detail Response (includes status fields)
            CreateMap<TrainerProfile, TrainerProfileDetailResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileDetailResponse>, string?>(tp => tp.CoverImageUrl))
                .ForMember(dest => dest.VideoIntroUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileDetailResponse>, string?>(tp => tp.VideoIntroUrl))
                .ForMember(dest => dest.StatusImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileDetailResponse>, string?>(tp => tp.StatusImageUrl));

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

           // Trainer Info (من TrainerProfile)
           .ForMember(d => d.TrainerId,
               o => o.MapFrom(s => s.Package.Trainer.UserId))
           .ForMember(d => d.TrainerName,
               o => o.MapFrom(s => s.Package.Trainer.User.FullName))
           .ForMember(d => d.TrainerHandle,
               o => o.MapFrom(s => s.Package.Trainer.Handle))
           .ForMember(d => d.TrainerPhotoUrl,
               o => o.MapFrom(s => s.Package.Trainer.User.ProfilePhotoUrl))
           .ForMember(d => d.IsAnnual,
               o => o.MapFrom(s =>
                   (s.CurrentPeriodEnd - s.StartDate).Days > 31));



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

            //start amr mapping


            // ======================
            // Trainer / Profile mappings
            // Used by:
            // - TrainerProfileController (GetAllProfiles, GetById, Create, Update, Delete)
            // - ProfileController (GetTrainerProfileById, UpdateTrainerProfile)
            // - HomeClientService / client endpoints to display trainer data
            // ======================
            CreateMap<TrainerProfile, TrainerProfileResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileResponse>, string?>(tp => tp.CoverImageUrl))
                .ForMember(dest => dest.VideoIntroUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileResponse>, string?>(tp => tp.VideoIntroUrl))
                .ForMember(dest => dest.StatusImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileResponse>, string?>(tp => tp.StatusImageUrl));

            // ======================
            // Program mappings
            // Used by:
            // - ProgramsController (GET/POST/PUT/DELETE/Search)
            // - ProgramManagerService / ProgramService
            // - HomeClientController for client-facing program responses
            // ======================
            CreateMap<Program, ProgramGetAllResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.User.UserName : null))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null))
                .ForMember(dest => dest.TrainerProfileId, opt => opt.MapFrom(p => p.TrainerProfileId))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Program, ProgramGetAllResponse>, string?>(p => p.ThumbnailUrl));

            CreateMap<Program, ProgramGetByIdResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.User.UserName : null))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null))
                .ForMember(dest => dest.TrainerProfileId, opt => opt.MapFrom(p => p.TrainerProfileId))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Program, ProgramGetByIdResponse>, string?>(p => p.ThumbnailUrl));

            // Program -> ProgramResponse (used by ProgramService)
            CreateMap<Program, ProgramResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.User.UserName : null))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null))
                .ForMember(dest => dest.TrainerProfileId, opt => opt.MapFrom(p => p.TrainerProfileId))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Program, ProgramResponse>, string?>(p => p.ThumbnailUrl));

            CreateMap<Program, DTOs.Client.ProgramClientResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.User.UserName : null))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null))
                .ForMember(dest => dest.TrainerProfileId, opt => opt.MapFrom(p => p.TrainerProfileId))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Program, DTOs.Client.ProgramClientResponse>, string?>(p => p.ThumbnailUrl));

            // ======================
            // Week / Day / DayExercise mappings
            // Used by:
            // - WeeksController (GET/POST/PUT/DELETE)
            // - DaysController (GET/POST/PUT/DELETE)
            // - DayExercisesController (GET/POST/PUT/DELETE)
            // ======================
            CreateMap<ProgramWeek, ProgramWeekGetAllResponse>();
            CreateMap<ProgramDay, ProgramDayGetAllResponse>();
            CreateMap<ProgramDayExercise, ProgramDayExerciseGetAllResponse> ()
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom<GenericImageUrlResolver<ProgramDayExercise, ProgramDayExerciseGetAllResponse>, string?>(p => p.VideoUrl));

            // ======================
            // Exercise Library mappings
            // Used by:
            // - ExerciseLibraryController (GetAll, GetById, Create, Update, Delete, Search)
            // ======================
            CreateMap<Exercise, ExerciseGetAllResponse>()
                .ForMember(dest => dest.VideoDemoUrl, opt => opt.MapFrom<GenericImageUrlResolver<Exercise, ExerciseGetAllResponse>, string?>(e => e.VideoDemoUrl))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Exercise, ExerciseGetAllResponse>, string?>(e => e.ThumbnailUrl));
            CreateMap<Exercise, ExerciseGetByIdResponse>()
                .ForMember(dest => dest.VideoDemoUrl, opt => opt.MapFrom<GenericImageUrlResolver<Exercise, ExerciseGetByIdResponse>, string?>(e => e.VideoDemoUrl))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Exercise, ExerciseGetByIdResponse>, string?>(e => e.ThumbnailUrl));
            CreateMap<ExerciseCreateRequest, Exercise>();
            CreateMap<ExerciseUpdateRequest, Exercise>();


            // ======================
            // TrainerProfile detailed mapping
            // Used by:
            // - TrainerProfileController (GetById) and ProfileController
            // ======================
            CreateMap<TrainerProfile, TrainerProfileGetResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(tp => tp.UserId))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileGetResponse>, string?>(tp => tp.CoverImageUrl))
                .ForMember(dest => dest.VideoIntroUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileGetResponse>, string?>(tp => tp.VideoIntroUrl))
                .ForMember(dest => dest.StatusImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, TrainerProfileGetResponse>, string?>(tp => tp.StatusImageUrl));

            // ======================
            // AppUser / Client mappings
            // Used by:
            // - HomeClientController (trainers/programs display)
            // - Admin user management endpoints
            // ======================
            CreateMap<AppUser, ClientGetAllResponse>()
                .ForMember(d => d.UserId, o => o.MapFrom(u => u.Id))
                .ForMember(d => d.UserName, o => o.MapFrom(u => u.UserName))
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom<GenericImageUrlResolver<AppUser, ClientGetAllResponse>, string?>(u => u.ProfilePhotoUrl));

            CreateMap<AppUser, ClientGetByIdResponse>()
                .ForMember(d => d.UserId, o => o.MapFrom(u => u.Id))
                .ForMember(d => d.UserName, o => o.MapFrom(u => u.UserName))
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom<GenericImageUrlResolver<AppUser, ClientGetByIdResponse>, string?>(u => u.ProfilePhotoUrl));


            // ======================
            // Package mappings (updated)
            // Used by:
            // - PackagesController / PackageService
            // - HomeClientService for client-facing package responses
            // ======================
            CreateMap<Package, PackageResponse>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(p => new DateTimeOffset(p.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(p => p.UpdatedAt))
                .ForMember(dest => dest.IsAnnual, opt => opt.MapFrom(p => p.IsAnnual))
                .ForMember(dest => dest.PromoCode, opt => opt.MapFrom(p => p.PromoCode))
                .ForMember(dest => dest.ProgramIds, opt => opt.MapFrom(p => p.PackagePrograms != null ? p.PackagePrograms.Where(pp => !pp.IsDeleted).Select(pp => pp.ProgramId).ToArray() : new int[0]))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Package, PackageResponse>, string?>(p => p.ThumbnailUrl));

            CreateMap<PackageCreateRequest, Package>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Trainer, opt => opt.Ignore())
                .ForMember(dest => dest.PackagePrograms, opt => opt.Ignore())
                .ForMember(dest => dest.Subscriptions, opt => opt.Ignore());
            CreateMap<PackageUpdateRequest, Package>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Client-facing package mapping
            CreateMap<Package, DTOs.Client.PackageClientResponse>()
                .ForMember(dest => dest.TrainerId, opt => opt.MapFrom(p => p.TrainerId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(p => new DateTimeOffset(p.CreatedAt)))
                .ForMember(dest => dest.IsAnnual, opt => opt.MapFrom(p => p.IsAnnual))
                .ForMember(dest => dest.PromoCode, opt => opt.MapFrom(p => p.PromoCode))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom<GenericImageUrlResolver<Package, DTOs.Client.PackageClientResponse>, string?>(p => p.ThumbnailUrl));


            // ======================
            // Client-facing Trainer mapping (detwailed fields)
            // Used by:
            // - HomeClientController / HomeClientService for trainer detail in client UI
            // ======================
            CreateMap<TrainerProfile, DTOs.Client.TrainerClientResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(tp => tp.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName))
                .ForMember(dest => dest.Handle, opt => opt.MapFrom(tp => tp.Handle))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(tp => tp.Bio))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom<GenericImageUrlResolver<TrainerProfile, DTOs.Client.TrainerClientResponse>, string?>(tp => tp.CoverImageUrl))
                .ForMember(dest => dest.RatingAverage, opt => opt.MapFrom(tp => tp.RatingAverage))
                .ForMember(dest => dest.TotalClients, opt => opt.MapFrom(tp => tp.TotalClients))
                .ForMember(dest => dest.YearsExperience, opt => opt.MapFrom(tp => tp.YearsExperience));


            // Reviews mappings
            CreateMap<TrainerReview, TrainerAreaReviewResponse>();
            CreateMap<TrainerReview, DTOs.Client.TrainerReviewClientResponse>();
            CreateMap<TrainerReview, DTOs.Trainer.TrainerReviewResponse>();
            CreateMap<TrainerReview, GuestReviewResponseItem>()
                .ForMember(dest => dest.ClientUserName, opt => opt.MapFrom(r => r.Client != null ? r.Client.User.UserName : string.Empty));

            CreateMap<TrainerProfile, TopTrainerResponse>()
                .ForMember(dest => dest.TrainerProfileId, opt => opt.MapFrom(tp => tp.Id))
                .ForMember(dest => dest.Handle, opt => opt.MapFrom(tp => tp.Handle))
                .ForMember(dest => dest.TotalClients, opt => opt.MapFrom(tp => tp.TotalClients));

            CreateMap<TrainerReview, AdminReviewActionResponse>()
                .ForMember(dest => dest.Message, opt => opt.Ignore());


            //end amr mapping



        }

    }
}