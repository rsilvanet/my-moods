using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IFormsService
    {
        Task<Form> GetByIdAsync(string id);
        Task<FormWithQuestionsDTO> GetWithQuestionsAsync(Form form);
        Task<IList<Form>> GetByCompanyAsync(string companyId, bool onlyActives);
        Task<FormMetadataDTO> GetMetadataByIdAsync(string id);
        Task InsertAsync(Form form);
        Task UpdateAsync(Form form);
        Task EnableAsync(Form form);
        Task DisableAsync(Form form);

        Task<ValidationResultDTO<Form>> ValidateToInsertAsync(string companyId, FormOnPostDTO dto);
        Task<ValidationResultDTO<Form>> ValidateToUpdateAsync(Form form, FormOnPutDTO dto);
    }
}
