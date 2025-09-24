using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;

namespace Businnes_Layer.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();

        Owner GetOwner(int  ownerId);

        ICollection<Owner> GetOwnerofPokemon(int  PokeId);

        ICollection<Pokemon> GetPokemonByOwner( int  ownerId);

        bool OwnerExists (int ownerId);

        bool CreateOwner(Owner owner);

        bool UpdateOwner(Owner owner);

        bool DeleteOwner(Owner owner);

        bool Save();

    }
}
