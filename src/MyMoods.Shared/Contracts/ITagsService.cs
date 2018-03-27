using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Shared.Contracts
{
    public interface ITagsService
    {
        Task<Tagg> GetByIdAsync(string id);
        Task<IList<Tagg>> GetDefaultsAsync(bool onlyActives);
        Task<IList<Tagg>> GetByCompanyAsync(string companyId, bool onlyActives);
        Task<IList<Tagg>> GetByFormAsync(Form form, bool onlyActives);
        Task<IList<Tagg>> GetOnlyCustomByFormAsync(Form form, bool onlyActives);
        Task InsertAsync(Tagg tag);
        Task EnableAsync(Tagg tag);
        Task DisableAsync(Tagg tag);
        Task<ValidationResultDTO<Tagg>> ValidateToInsertAsync(string companyId, TagOnPostDTO dto);
    }
}
