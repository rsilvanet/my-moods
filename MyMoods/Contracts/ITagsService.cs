using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface ITagsService
    {
        Task<Tagg> GetByIdAsync(string id);
        Task<IList<Tagg>> GetByCompanyAsync(string companyId);
        Task<IList<Tagg>> GetByFormAsync(Form form);
        Task InsertAsync(Tagg tag);
        Task<ValidationResultDTO<Tagg>> ValidateToInsertAsync(string companyId, TagOnPostDTO dto);
        Task EnableAsync(Tagg tag);
        Task DisableAsync(Tagg tag);
    }
}
