using AgendaApi.Application.Contacts.Commands;
using AgendaApi.Application.Contacts.DTOs;
using AgendaApi.Domain.Entities;
using AutoMapper;

namespace AgendaApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>();

            CreateMap<CreateContactCommand, Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateContactCommand, Contact>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }

}
