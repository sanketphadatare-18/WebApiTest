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
    public class OwnerController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _owner;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository owner, IMapper mapper, ICountryRepository countryRepository)
        {
            _owner = owner;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public ActionResult GetOwners()
        {
            var owner = _mapper.Map<List<OwnrVM>>(_owner.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public ActionResult GetOwner(int ownerId)
        {
            if (!_owner.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnrVM>(_owner.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);

        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public ActionResult GetPokemonByOwner(int ownerId)
        {
            if(!_owner.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = _mapper.Map<List<PokemonVM>>(_owner.GetPokemonByOwner(ownerId));    
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateOwner([FromQuery]int countryId ,[FromBody] OwnrVM ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var owners = _owner.GetOwners()
                .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (owners != null)
            {
                ModelState.AddModelError("", " Owner Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountryById(countryId);

            if (!_owner.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("SucessFully Created");


        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdateOwner(int ownerId, [FromBody] OwnrVM updateOwner)
        {
            if (updateOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updateOwner.Id)
                return BadRequest(ModelState);

            if (!_owner.OwnerExists(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(updateOwner);

            if (!_owner.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Somithing Went wrong while updating Owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteCategory(int ownerId)
        {


            if (!_owner.OwnerExists(ownerId))
                return NotFound();

            var ownerToDelete = _owner.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_owner.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete this owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
