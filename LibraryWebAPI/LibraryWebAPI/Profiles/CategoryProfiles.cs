using AutoMapper;
using LibraryDataAccess.Models;
using LibraryWebAPI.Dto.Category;
using Libray.Core;

namespace LibraryWebAPI.Profiles
{
    public class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<PaginatedList<Category>, PaginatedList<CategoryDto>>();
        }
    }
}
