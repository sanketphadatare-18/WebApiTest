using Businnes_Layer.Repositories.Interfaces;
using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;
using Web_Api_Core_.Model.Joins_Tables;

namespace Businnes_Layer.Repositories.Implimentation
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ApplicationDbContext _context;
        public PokemonRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonown = _context.Owners.Where( a => a.Id == ownerId ).FirstOrDefault();
            var category = _context.Categories.Where( i => i.Id == categoryId ).FirstOrDefault();

            var pokeOwnerr = new PokemonOwner()
            {
                Owner = pokemonown,
                Pokemon = pokemon,
            };

            _context.Add(pokeOwnerr);

            var pokemoncategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _context.Add(pokemoncategory);

            _context.Add(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemonid)
        {
            _context.Remove(pokemonid);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(c => c.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(c => c.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeid)
        {
            var reviews = _context.Reviews
                .Where(p => p.Pokemon.Id == pokeid)
                .ToList();

            if (reviews == null || !reviews.Any())
                return 0;

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);  
            return Save();
        }
    }
}
