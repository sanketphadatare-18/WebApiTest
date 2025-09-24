using Businnes_Layer.Repositories.Interfaces;
using Data_Layer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;

namespace Businnes_Layer.Repositories.Implimentation
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext _context;


        public OwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(c => c.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerofPokemon(int PokeId)
        {
            return _context.PokemonOwners.Where(o => o.Pokemon.Id == PokeId)
                .Select(c => c.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p => p.OwnerId == ownerId)
                .Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(c => c.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
