using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IReviewsService
    {
        Task<Review> GetByIdAsync(string id);
        Task<IList<ReviewDTO>> GetByFormAsync(Form form, DateTime startDate, DateTime endDate, short timezone);
        Task<IList<ReviewsResumeDTO>> GetResumeAsync(Form form, short timezone);
        Task<IList<ReviewsDetailedByMoodDTO>> GetDayDetailedByMoodAsync(Form form, DateTime date, short timezone);
        Task<IList<MoodCounterDTO>> GetMoodsCounterAsync(Form form, DateTime startDate, DateTime endDate, short timezone);
        Task<IList<TagCounterWithMoodsDTO>> GetTagsCounterAsync(Form form, DateTime startDate, DateTime endDate, short timezone);
        Task<IList<MaslowCounterDTO>> GetMaslowCounterAsync(Form form, DateTime startDate, DateTime endDate, short timezone);
        Task InsertAsync(Review review);
        Task InsertManyAsync(IList<Review> reviews);
        Task EnableAsync(Review review);
        Task DisableAsync(Review review);
        Task<ValidationResultDTO<Review>> ValidateToInsertAsync(Form form, ReviewOnPostDTO review);
    }
}
