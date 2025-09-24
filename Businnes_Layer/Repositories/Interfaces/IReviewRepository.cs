using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;

namespace Businnes_Layer.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();

        Review GetReview(int reviewid);

        ICollection<Review>GetreviewsOfPokemon(int pokeId);

        bool ReviewExists (int reviewid);

        bool CreateReview (Review review);

        bool UpdateReview(Review review);

        bool DeleteReview(Review review);

        bool DeleteReviews(List<Review> reviews);

        bool Save();


    }
}
