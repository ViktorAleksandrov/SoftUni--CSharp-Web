using System.Globalization;
using AutoMapper;
using Eventures.Models;
using Eventures.Web.ViewModels.Account;
using Eventures.Web.ViewModels.Events;
using Eventures.Web.ViewModels.Orders;

namespace Eventures.Web.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<RegisterUserBindingModel, User>();

            this.CreateMap<CreateEventBindingModel, Event>();

            this.CreateMap<CreateOrderBindingModel, Order>();

            this.CreateMap<User, UserViewModel>();

            this.CreateMap<Event, AllEventsViewModel>()
                .ForMember(
                    dest => dest.Start,
                    opt => opt.MapFrom(
                        source => source.Start.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.End,
                    opt => opt.MapFrom(
                        source => source.End.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)));

            this.CreateMap<Order, MyEventsViewModel>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(
                        source => source.Event.Name))
                .ForMember(
                    dest => dest.Start,
                    opt => opt.MapFrom(
                        source => source.Event.Start.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.End,
                    opt => opt.MapFrom(
                        source => source.Event.End.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)));

            this.CreateMap<Order, AllOrdersViewModel>()
                .ForMember(
                    dest => dest.Event,
                    opt => opt.MapFrom(
                        source => source.Event.Name))
                .ForMember(
                    dest => dest.Customer,
                    opt => opt.MapFrom(
                        source => source.Customer.UserName))
                .ForMember(
                    dest => dest.OrderedOn,
                    opt => opt.MapFrom(
                        source => source.OrderedOn.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture)));
        }
    }
}
