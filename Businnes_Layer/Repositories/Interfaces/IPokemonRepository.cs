using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;

namespace Businnes_Layer.Repositories.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);

        decimal GetPokemonRating(int pokeid);

        bool PokemonExists(int pokeId);

        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool DeletePokemon(Pokemon pokemonid);


        bool Save();

    }
}
