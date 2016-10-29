using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IReviewsService
    {
        Task InsertAsync(Review review);
        Task<ValidationResultDTO<Review>> ValidateToInsertAsync(Form form, ReviewOnPostDTO review);
    }
}
