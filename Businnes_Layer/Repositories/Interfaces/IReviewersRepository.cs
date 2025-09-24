using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;

namespace Businnes_Layer.Repositories.Interfaces
{
    public interface IReviewersRepository
    {
        ICollection<Reviewer> GetReviewers();

        Reviewer GetReviewer(int id);

        ICollection<Review> GetReviewesByReviewers( int reviewerId);

        bool ReveiewerExists(int reviewerId);

        bool CreateReviewer (Reviewer reviewer);

        bool UpdateReviewer (Reviewer reviewer);

        bool DeleteReviewer (Reviewer reviewer);

        bool Save();


    }
}
