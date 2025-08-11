namespace Quiz.CSharp.Api.Mapping;

using AutoMapper;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Data.Entities;

public sealed class CollectionProfile : Profile
{
    public CollectionProfile()
    {
        CreateMap<Collection, CollectionResponse>()
            .ForMember(dest => dest.UserProgress, opt => opt.Ignore());

        CreateMap<Collection, CreateCollectionResponse>()
            .ForMember(dest => dest.QuestionsCreated,
                opt => opt.MapFrom(src => src.Questions != null ? src.Questions.Count : 0));


        CreateMap<UserProgress, UserProgressResponse>()
            .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src =>
                src.TotalQuestions > 0 ? (decimal)src.AnsweredQuestions / src.TotalQuestions * 100 : 0));

        CreateMap<UserProgress, UserProgressManagementResponse>()
            .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src =>
                src.TotalQuestions > 0 ? (decimal)src.AnsweredQuestions / src.TotalQuestions * 100 : 0));

        CreateMap<UserProgress, CollectionProgressResponse>()
            .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src =>
                src.TotalQuestions > 0 ? (decimal)src.AnsweredQuestions / src.TotalQuestions * 100 : 0));

        CreateMap<(ICurrentUser currentUser, int TotalQuestions, int AnsweredQuestions, int CorrectAnswers, decimal SuccessRate, int CollectionId, string UserId), UserProgress>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.currentUser.Username))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.currentUser.Name))
            .ForMember(dest => dest.TelegramUsername, opt => opt.MapFrom(src => src.currentUser.TelegramUsername))
            .ForMember(dest => dest.LastAnsweredAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<CreateCollectionRequest, Collection>()
            .ForMember(dest => dest.Questions, opt => opt.Ignore());

    }
} 