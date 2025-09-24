using AutoMapper;
using Data_Layer.Model;
using Web_Api_Core_.DTO;
using Web_Api_Core_.Model;

namespace Web_Api_Core_.Helper
{
    public class MyMappingProfile: Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Pokemon, PokemonVM>();
            CreateMap<PokemonVM, Pokemon>();

            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryVM, Category>();

            CreateMap<Country, CountryVM>();
            CreateMap<CountryVM, Country>();

            CreateMap<Owner, OwnrVM>();
            CreateMap<OwnrVM, Owner>();

            CreateMap<Review, ReviewVM>();
            CreateMap<ReviewVM, Review>();

            CreateMap<Reviewer, ReviewerVM>();
            CreateMap<ReviewerVM, Reviewer>();
        }
    }
}
