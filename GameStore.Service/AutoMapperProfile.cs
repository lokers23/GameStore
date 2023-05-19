using AutoMapper;
using GameStore.Domain.Dto.Activation;
using GameStore.Domain.Dto.Developer;
using GameStore.Domain.Dto.Game;
using GameStore.Domain.Dto.Genre;
using GameStore.Domain.Dto.Image;
using GameStore.Domain.Dto.Key;
using GameStore.Domain.Dto.MinimumSpecification;
using GameStore.Domain.Dto.Order;
using GameStore.Domain.Dto.Platform;
using GameStore.Domain.Dto.Publisher;
using GameStore.Domain.Dto.User;
using GameStore.Domain.Models;
using GameStore.Domain.ViewModels.Account;
using GameStore.Domain.ViewModels.Activation;
using GameStore.Domain.ViewModels.Developer;
using GameStore.Domain.ViewModels.Game;
using GameStore.Domain.ViewModels.Genre;
using GameStore.Domain.ViewModels.Image;
using GameStore.Domain.ViewModels.Key;
using GameStore.Domain.ViewModels.MinimumSpecification;
using GameStore.Domain.ViewModels.Order;
using GameStore.Domain.ViewModels.Platform;
using GameStore.Domain.ViewModels.Publisher;

namespace GameStore.Service;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        #region ViewtoDALModel
        CreateMap<MinSpecificationViewModel, MinimumSpecification>();
        CreateMap<PlatformViewModel, Platform>();
        CreateMap<ActivationViewModel, Activation>();
        CreateMap<DeveloperViewModel, Developer>();
        CreateMap<GenreViewModel, Genre>();
        CreateMap<ImageViewModel, Image>();
        CreateMap<KeyViewModel, Key>();
        CreateMap<OrderViewModel, Order>();
        CreateMap<PublisherViewModel, Publisher>();
        CreateMap<RegistrationViewModel, User>();
        CreateMap<GameViewModel, Game>();
        #endregion
        
        #region DAlModeltoDto
        CreateMap<MinimumSpecification, MinSpecDto>();
        CreateMap<Platform, PlatformDto>();
        CreateMap<Activation, ActivationDto>();
        CreateMap<Developer, DeveloperDto>();
        CreateMap<Genre, GenreDto>();
        CreateMap<Image, ImageDto>();
        CreateMap<Key, KeyDto>();
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Keys, 
                opt => opt.MapFrom(src => src.KeyOrders.Select(ko => ko.Key)));
        CreateMap<Publisher, PublisherDto>();
        CreateMap<User, UserDto>();
        CreateMap<User, UserShortDto>();
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.GameGenres.Select(ko => ko.Genre)))
            .ForMember(dest => dest.MinimumSpecifications,
                opt => opt.MapFrom(src => src.GameMinSpecifications.Select(ko => ko.MinimumSpecification)));
        #endregion

        #region DtoToModel
        CreateMap<KeyDto, Key>();
        CreateMap<GameDto, Game>();
        CreateMap<ActivationDto, Activation>();
        #endregion
    }
}