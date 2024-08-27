using AutoMapper;
using LibraryDataAccess.Models;
using LibraryWebAPI.Dto.Author;
using Libray.Core;

namespace LibraryWebAPI.Profiles
{
    public class AuthorProfiles : Profile
    {
        public AuthorProfiles()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(a => a.Id, opt => opt.MapFrom(a => a.AuthorId))
                .ForMember(a => a.Name, opt => opt.MapFrom(a => a.AuthorName))
                .ReverseMap();
            CreateMap<CreateAuthorDto, Author>()
                .ForMember(a => a.AuthorName, opt => opt.MapFrom(a => a.Name))
                .ReverseMap();
            CreateMap<PaginatedList<Author>, PaginatedList<AuthorDto>>();
        }
    }
}
