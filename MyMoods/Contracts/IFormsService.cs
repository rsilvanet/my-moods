using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IFormsService
    {
        Task<Form> GetByIdAsync(string id);
        Task<IList<Form>> GetByCompanyAsync(string companyId);
        Task<FormMetadataDTO> GetMetadataByIdAsync(string id);
        Task<Form> GenerateFormAsync(string companyId, string title, bool useDefaultTags);
        Task UpdateFormAsync(Form form, string title, bool useDefaultTags);
    }
}
