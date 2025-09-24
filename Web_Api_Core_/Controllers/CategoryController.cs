using AutoMapper;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public ActionResult GetCategories()
        {
            var Categ = _mapper.Map<List<CategoryVM>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(Categ);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public ActionResult GetPokemon(int CategId)
        {
            if (!_categoryRepository.CategoryExists(CategId))
                return NotFound();

            var Category = _mapper.Map<CategoryVM>(_categoryRepository.GetCategory(CategId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Category);

        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public ActionResult GetPokemonByCategory(int CategId)
        {
            var pokemons = _mapper.Map<List<PokemonVM>>(
                _categoryRepository.GetPokemonByCategory(CategId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateCategory([FromBody] CategoryVM categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }
            var getCategory = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (getCategory != null)
            {
                ModelState.AddModelError("", " Cetegory Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("SucessFully Created");


        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdateCategory(int categoryId, [FromBody] CategoryVM updatedcategory)
        {
            if (updatedcategory == null)
                return BadRequest(ModelState);

            if (categoryId != updatedcategory.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(updatedcategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Somithing Went wrong while updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteCategory(int categoryId)
        {
   

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

           if(!ModelState.IsValid)
                return BadRequest(ModelState);

           if(!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("","Something went wrong while trying to delete this category");
                return StatusCode(500, ModelState);
            }

           return NoContent();
        }
    


   }
}
