﻿using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;

namespace MyMoods.Contracts
{
    public interface IMoodsService
    {
        IList<MoodDTO> GetMoods();
        string GetImage(MoodType mood);
        string GetTagsHelpText(MoodType mood);
        MoodType GetMoodByPoints(double points);
    }
}
