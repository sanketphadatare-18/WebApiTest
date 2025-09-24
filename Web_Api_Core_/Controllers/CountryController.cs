using AutoMapper;
using Businnes_Layer.Repositories.Implimentation;
using Businnes_Layer.Repositories.Interfaces;
using Data_Layer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Core_.DTO;
using Web_Api_Core_.Model;

namespace Web_Api_Core_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly   ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public ActionResult GetAllCountries()
        {
            var country = _mapper.Map<List<CountryVM>>(_countryRepository.GetAllCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public ActionResult GetCountryById(int countryId)
        {
            if (!_countryRepository.countryExists(countryId))
                return NotFound();

            var country = _mapper.Map<CountryVM>(_countryRepository.GetCountryById(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);

        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public ActionResult GetCountryByOwner(int ownerId)
        {
           var country = _mapper.Map<CountryVM>(_countryRepository.GetCountryByOwner(ownerId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateCountry([FromBody] CountryVM countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }
            var country = _countryRepository.GetAllCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", " Country Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countrymap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countrymap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("SucessFully Created");


        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdateCountry(int countryId, [FromBody] CountryVM updateCountry)
        {
            if (updateCountry == null)
                return BadRequest(ModelState);

            if (countryId != updateCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.countryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(updateCountry);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Somithing Went wrong while updating Country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteCategory(int countryId)
        {


            if (!_countryRepository.countryExists(countryId))
                return NotFound();

            var countrytoDelete = _countryRepository.GetCountryById(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countrytoDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete this country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
