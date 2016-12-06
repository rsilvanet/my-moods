﻿using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IReviewsService
    {
        Task InsertAsync(Review review);
        Task<ValidationResultDTO<Review>> ValidateToInsertAsync(Form form, ReviewOnPostDTO review);
        Task<IList<ReviewDTO>> GetByForm(Form form, DateTime date, short timezone);
        Task<IList<DailySimpleDTO>> GetResumeAsync(Form form, short timezone);
        Task<IList<DailyDetailedDTO>> GetDailyAsync(Form form, DateTime date, short timezone);
    }
}
