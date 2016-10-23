﻿namespace MyMoods.Domain.DTO
{
    public class TagDTO
    {
        public TagDTO(Tagg tag)
        {
            Id = tag.Id.ToString();
            Title = tag.Title;
            Type = tag.Type;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public TagType Type { get; set; }
    }
}
