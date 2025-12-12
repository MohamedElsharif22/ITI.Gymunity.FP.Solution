using AutoMapper;
using ITI.Gymunity.FP.Application.DTOs.Trainer;
using ITI.Gymunity.FP.Application.DTOs.Program;
using ITI.Gymunity.FP.Domain.Models.Trainer;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;

namespace ITI.Gymunity.FP.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ========== Trainer Profile Mappings ==========
            CreateMap<TrainerProfile, TrainerProfileListResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));

            CreateMap<TrainerProfile, TrainerProfileDetailResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(tp => tp.User.UserName));

            CreateMap<CreateTrainerProfileRequest, TrainerProfile>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDescription, opt => opt.Ignore());

            CreateMap<UpdateTrainerProfileRequest, TrainerProfile>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDescription, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========== Program Mappings ==========
            CreateMap<CreateProgramRequest, Domain.Models.ProgramAggregate.Program>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateProgramRequest, Domain.Models.ProgramAggregate.Program>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Domain.Models.ProgramAggregate.Program, ProgramResponse>()
                .ForMember(dest => dest.TotalWeeks, opt => opt.MapFrom(src => src.Weeks.Count));

            CreateMap<Domain.Models.ProgramAggregate.Program, ProgramDetailResponse>()
                .ForMember(dest => dest.TotalWeeks, opt => opt.MapFrom(src => src.Weeks.Count))
                .ForMember(dest => dest.Weeks, opt => opt.MapFrom(src => src.Weeks));

            // ========== Program Week Mappings ==========
            CreateMap<ProgramWeek, ProgramWeekResponse>()
                .ForMember(dest => dest.TotalDays, opt => opt.MapFrom(src => src.Days.Count));

            CreateMap<ProgramWeek, ProgramWeekDetailResponse>()
                .ForMember(dest => dest.TotalDays, opt => opt.MapFrom(src => src.Days.Count))
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.Days));

            // ========== Program Day Mappings ==========
            CreateMap<ProgramDay, ProgramDayResponse>()
                .ForMember(dest => dest.TotalExercises, opt => opt.MapFrom(src => src.Exercises.Count));

            CreateMap<ProgramDay, ProgramDayDetailResponse>()
                .ForMember(dest => dest.TotalExercises, opt => opt.MapFrom(src => src.Exercises.Count))
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));

            // ========== Program Day Exercise Mappings ==========
            CreateMap<AddExerciseToDayRequest, ProgramDayExercise>();

            CreateMap<UpdateExerciseInDayRequest, ProgramDayExercise>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProgramDayExercise, ProgramDayExerciseResponse>()
                .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Exercise.Category))
                .ForMember(dest => dest.MuscleGroup, opt => opt.MapFrom(src => src.Exercise.MuscleGroup));

            // ========== Exercise Library Mappings ==========
            CreateMap<CreateExerciseRequest, Exercise>()
                .ForMember(dest => dest.IsCustom, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false));

            CreateMap<Exercise, ExerciseResponse>();
        }
    }
}