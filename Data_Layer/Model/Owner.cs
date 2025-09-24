using Web_Api_Core_.Model.Joins_Tables;

namespace Web_Api_Core_.Model
{
    public class Owner
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string GYM { get; set; }

        public Country Country { get; set; }

        public ICollection<PokemonOwner> PokemonOwner { get; set; }


        
    }
}
