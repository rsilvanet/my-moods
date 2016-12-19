﻿using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class TagsService
    {
        private readonly IStorage _storage;

        public TagsService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task InsertAsync(Tagg tag)
        {
            if (!tag.ValidateWasCalled())
            {
                throw new InvalidOperationException("O objeto não foi previamente validado.");
            }

            await _storage.Tags.InsertOneAsync(tag);
        }

        public Task<ValidationResultDTO<Tagg>> ValidateToInsertAsync(Company company, TagOnPostDTO dto)
        {
            var result = new ValidationResultDTO<Tagg>();

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }
            else
            {
                result.ParsedObject.Title = dto.Title;
            }

            if (string.IsNullOrWhiteSpace(dto.Type))
            {
                result.Error("type", "Tipo não selecionado.");
            }
            else
            {
                TagType type;

                if (Enum.TryParse(dto.Type, out type))
                {
                    result.ParsedObject.Type = type;
                }
                else
                {
                    result.Error("type", $"Tipo {dto.Type} inválido.");
                }
            }

            result.ParsedObject.Validate();

            return Task.FromResult(result);
        }
    }
}
