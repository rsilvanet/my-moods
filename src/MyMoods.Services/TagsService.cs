using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class TagsService : ITagsService
    {
        private readonly IStorage _storage;

        public TagsService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<Tagg> GetByIdAsync(string id)
        {
            var oid = new ObjectId(id);
            var tag = await _storage.Tags.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            return tag;
        }

        public async Task<IList<Tagg>> GetDefaultsAsync(bool onlyActives)
        {
            var tags = await _storage.Tags.Find(x => x.Company == null).ToListAsync();

            if (onlyActives)
            {
                return tags.Where(x => x.Active).ToList();
            }

            return tags;
        }

        public async Task<IList<Tagg>> GetByCompanyAsync(string companyId, bool onlyActives)
        {
            var oid = new ObjectId(companyId);
            var tags = await _storage.Tags.Find(x => x.Company.Equals(oid)).ToListAsync();

            if (onlyActives)
            {
                tags = tags.Where(x => x.Active).ToList();
            }

            return tags;
        }

        public async Task<IList<Tagg>> GetOnlyCustomByFormAsync(Form form, bool onlyActives)
        {
            var companyTags = await _storage.Tags.Find(x => x.Company.Equals(form.Company)).ToListAsync();
            var customTags = companyTags.Where(x => form.CustomTags.Any(z => z.ToString() == x.Id.ToString())).ToList();

            if (onlyActives)
            {
                customTags = customTags.Where(x => x.Active).ToList();
            }

            return customTags;
        }

        public async Task<IList<Tagg>> GetByFormAsync(Form form, bool onlyActives)
        {
            switch (form.Type)
            {
                case FormType.general:
                    {
                        return await GetDefaultsAsync(onlyActives);
                    }
                case FormType.generalWithCustomTags:
                    {
                        var defaults = await GetDefaultsAsync(onlyActives);
                        var customs = await GetOnlyCustomByFormAsync(form, onlyActives);

                        return (defaults).Concat(customs).ToList();
                    }
                case FormType.generalOnlyCustomTags:
                    {
                        return await GetOnlyCustomByFormAsync(form, onlyActives);
                    }
                default:
                    {
                        return new List<Tagg>();
                    }
            }
        }

        public async Task InsertAsync(Tagg tag)
        {
            if (!tag.IsValidated())
            {
                throw new InvalidOperationException("Objeto inválido para gravação.");
            }

            await _storage.Tags.InsertOneAsync(tag);
        }

        public async Task EnableAsync(Tagg tag)
        {
            var builder = Builders<Tagg>.Update.Set(x => x.Active, true);

            await _storage.Tags.UpdateOneAsync(x => x.Id.Equals(tag.Id), builder);
        }

        public async Task DisableAsync(Tagg tag)
        {
            var builder = Builders<Tagg>.Update.Set(x => x.Active, false);

            await _storage.Tags.UpdateOneAsync(x => x.Id.Equals(tag.Id), builder);
        }

        public Task<ValidationResultDTO<Tagg>> ValidateToInsertAsync(string companyId, TagOnPostDTO dto)
        {
            var result = new ValidationResultDTO<Tagg>();

            result.ParsedObject.Active = true;
            result.ParsedObject.Company = new ObjectId(companyId);

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }
            else
            {
                result.ParsedObject.Title = dto.Title;
            }

            if (!dto.Type.HasValue)
            {
                result.Error("type", "Tipo não selecionado ou inválido.");
            }
            else
            {
                result.ParsedObject.Type = dto.Type.Value;
            }

            if (result.Success)
            {
                result.ParsedObject.Validate();
            }

            return Task.FromResult(result);
        }
    }
}
