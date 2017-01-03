using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Collections.Generic;
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

        public async Task<IList<Tagg>> GetByCompanyAsync(string companyId)
        {
            var companyOid = new ObjectId(companyId);
            var tags = await _storage.Tags.Find(x => x.Company.Equals(companyOid)).ToListAsync();

            return tags;
        }

        public async Task<IList<Tagg>> GetByFormAsync(Form form)
        {
            switch (form.Type)
            {
                case FormType.general:
                    return await _storage.Tags.Find(x => x.Company == null).ToListAsync();
                case FormType.generalWithCustomTags:
                    return await _storage.Tags.Find(x => x.Company == null || x.Company.Equals(form.Company)).ToListAsync();
                case FormType.generalOnlyCustomTags:
                    return await _storage.Tags.Find(x => x.Company.Equals(form.Company)).ToListAsync();
                default:
                    return new List<Tagg>();
            }
        }

        public async Task InsertAsync(Tagg tag)
        {
            if (!tag.ValidateWasCalled())
            {
                throw new InvalidOperationException("O objeto não foi previamente validado.");
            }

            await _storage.Tags.InsertOneAsync(tag);
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

            result.ParsedObject.Validate();

            return Task.FromResult(result);
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
    }
}
