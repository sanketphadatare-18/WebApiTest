using AutoMapper;
using Businnes_Layer.Repositories.Implimentation;
using Businnes_Layer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Core_.DTO;
using Web_Api_Core_.Model;

namespace Web_Api_Core_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IOwnerRepository _owner;
        private readonly IPokemonRepository _pokemonRepo;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _revieweRepo;



        public PokemonController(IPokemonRepository pokemonRepo, IMapper mapper, IOwnerRepository owner, IReviewRepository revieweRepo)
        {
            _owner = owner;
            _pokemonRepo = pokemonRepo;
            _mapper = mapper;
            _revieweRepo = revieweRepo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public ActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonVM>>(_pokemonRepo.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public ActionResult GetPokemon(int pokeId)
        {
            if(!_pokemonRepo.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonVM>(_pokemonRepo.GetPokemon(pokeId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);

        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public ActionResult GetPokemonRating(int pokeId)
        {
            if(!_pokemonRepo.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepo.GetPokemonRating(pokeId);
            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
            
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateOwner([FromQuery] int ownerId,[FromQuery] int categoryId, [FromBody] PokemonVM pokeCreate)
        {
            if (pokeCreate == null)
            {
                return BadRequest(ModelState);
            }
            var pokemon = _pokemonRepo.GetPokemons()
                .Where(c => c.Name.Trim().ToUpper() == pokeCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", " Pokemon Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokeCreate);

            if (!_pokemonRepo.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("SucessFully Created");


        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId,[FromQuery] int categoryId, [FromBody] PokemonVM updatepokemon)
        {
            if (updatepokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatepokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepo.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokeMap = _mapper.Map<Pokemon>(updatepokemon);

            if (!_pokemonRepo.UpdatePokemon(ownerId,categoryId,pokeMap))
            {
                ModelState.AddModelError("", "Somithing Went wrong while updating Pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeletePokemon(int pokeId)
        {


            if (!_pokemonRepo.PokemonExists(pokeId))
                return NotFound();

            var reviewsToDelete = _revieweRepo.GetreviewsOfPokemon(pokeId);
            var pokemondelete = _pokemonRepo.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_revieweRepo.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something gone worng while deleting reviews");
            }

            if (!_pokemonRepo.DeletePokemon(pokemondelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete this pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



    }
}
