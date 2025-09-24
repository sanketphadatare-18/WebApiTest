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
    public class ReviewersController : ControllerBase
    {
        private readonly IReviewersRepository _reviewersRepository;
        private readonly IMapper _mapper;

        public ReviewersController(IReviewersRepository reviewersRepository, IMapper mapper)
        {
            _reviewersRepository = reviewersRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public ActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerVM>>(_reviewersRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewersRepository.ReveiewerExists(reviewerId))
                return NotFound();

            var reviewr = _mapper.Map<ReviewerVM>(_reviewersRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewr);

        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewersRepository.ReveiewerExists(reviewerId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewVM>>
                (_reviewersRepository.GetReviewesByReviewers(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);

        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateReviwer( [FromBody] ReviewerVM reviewerCreate)
        {
            if (reviewerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var reviewer = _reviewersRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (reviewer != null)
            {
                ModelState.AddModelError("", " Reviewer Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);


            if (!_reviewersRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok("SucessFully Created");


        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerVM updatereviewer)
        {
            if (updatereviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatereviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewersRepository.ReveiewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(updatereviewer);

            if (!_reviewersRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Somithing Went wrong while updating reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteReviewer(int reviewerId)
        {


            if (!_reviewersRepository.ReveiewerExists(reviewerId))
                return NotFound();

            var reviewerToDelete = _reviewersRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewersRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while trying to delete this reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
