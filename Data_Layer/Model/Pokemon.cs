using Web_Api_Core_.Model.Joins_Tables;

namespace Web_Api_Core_.Model
{
    public class Pokemon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }


        //Joins User with ("Pokem" & "Reviewer")

        public ICollection<Review> Reviews { get; set; }

        public ICollection<PokemonOwner> PokemonOwner { get; set; }

        public ICollection <PokemonCategory> PokemonCategories  { get; set; }

       
    }
}
