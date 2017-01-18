﻿using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface ITagsService
    {
        Task<Tagg> GetByIdAsync(string id);
        Task<IList<Tagg>> GetDefaultsAsync(bool onlyActives);
        Task<IList<Tagg>> GetByCompanyAsync(string companyId, bool onlyActives);
        Task<IList<Tagg>> GetCustomByFormAsync(Form form, bool onlyActives);
        Task<IList<Tagg>> GetAllByFormAsync(Form form, bool onlyActives);
        Task InsertAsync(Tagg tag);
        Task EnableAsync(Tagg tag);
        Task DisableAsync(Tagg tag);
        Task<ValidationResultDTO<Tagg>> ValidateToInsertAsync(string companyId, TagOnPostDTO dto);
    }
}
