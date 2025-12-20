using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.Client;
using ITI.Gymunity.FP.Application.DTOs.ExerciseLibrary;
using ITI.Gymunity.FP.Application.DTOs.Package;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.Messaging;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Application.DTOs.Guest;
using ITI.Gymunity.FP.Application.DTOs.Admin;
using ITI.Gymunity.FP.Application.DTOs.Trainer;

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


            //start amr mapping


            // ======================
            // Trainer / Profile mappings
            // Used by:
            // - TrainerProfileController (GetAllProfiles, GetById, Create, Update, Delete)
            // - ProfileController (GetTrainerProfileById, UpdateTrainerProfile)
            // - HomeClientService / client endpoints to display trainer data
            // ======================
            CreateMap<TrainerProfile, TrainerProfileResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));

            // ======================
            // Program mappings
            // Used by:
            // - ProgramsController (GET/POST/PUT/DELETE/Search)
            // - ProgramManagerService / ProgramService
            // - HomeClientController for client-facing program responses
            // ======================
            CreateMap<Program, ProgramGetAllResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.Trainer.UserName))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null));

            CreateMap<Program, ProgramGetByIdResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.Trainer.UserName))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null));


            CreateMap<Program, DTOs.Client.ProgramClientResponse>()
                .ForMember(dest => dest.TrainerUserName, opt => opt.MapFrom(p => p.Trainer.UserName))
                .ForMember(dest => dest.TrainerHandle, opt => opt.MapFrom(p => p.TrainerProfile != null ? p.TrainerProfile.Handle : null));

            // ======================
            // Week / Day / DayExercise mappings
            // Used by:
            // - WeeksController (GET/POST/PUT/DELETE)
            // - DaysController (GET/POST/PUT/DELETE)
            // - DayExercisesController (GET/POST/PUT/DELETE)
            // ======================
            CreateMap<ProgramWeek, ProgramWeekGetAllResponse>();
            CreateMap<ProgramDay, ProgramDayGetAllResponse>();
            CreateMap<ProgramDayExercise, ProgramDayExerciseGetAllResponse>();

            // ======================
            // Exercise Library mappings
            // Used by:
            // - ExerciseLibraryController (GetAll, GetById, Create, Update, Delete, Search)
            // ======================
            CreateMap<Exercise, ExerciseGetAllResponse>();
            CreateMap<Exercise, ExerciseGetByIdResponse>();
            CreateMap<ExerciseCreateRequest, Exercise>();
            CreateMap<ExerciseUpdateRequest, Exercise>();

          

            // ======================
            // TrainerProfile detailed mapping
            // Used by:
            // - TrainerProfileController (GetById) and ProfileController
            // ======================
            CreateMap<TrainerProfile, TrainerProfileGetResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(tp => tp.UserId));

            // ======================
            // AppUser / Client mappings
            // Used by:
            // - HomeClientController (trainers/programs display)
            // - Admin user management endpoints
            // ======================
            CreateMap<AppUser, ClientGetAllResponse>()
                .ForMember(d => d.UserId, o => o.MapFrom(u => u.Id))
                .ForMember(d => d.UserName, o => o.MapFrom(u => u.UserName));

            CreateMap<AppUser, ClientGetByIdResponse>()
                .ForMember(d => d.UserId, o => o.MapFrom(u => u.Id))
                .ForMember(d => d.UserName, o => o.MapFrom(u => u.UserName));


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
                .ForMember(dest => dest.PromoCode, opt => opt.MapFrom(p => p.PromoCode));

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
                .ForMember(dest => dest.PromoCode, opt => opt.MapFrom(p => p.PromoCode));


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
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(tp => tp.CoverImageUrl))
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