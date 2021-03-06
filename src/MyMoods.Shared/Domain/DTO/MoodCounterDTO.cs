﻿using MyMoods.Shared.Domain.Enums;

namespace MyMoods.Shared.Domain.DTO
{
    public class MoodCounterDTO
    {
        public MoodCounterDTO(MoodType type, long count)
        {
            Mood = type;
            Count = count;
        }

        public MoodType Mood { get; private set; }
        public long Count { get; set; }
    }
}
