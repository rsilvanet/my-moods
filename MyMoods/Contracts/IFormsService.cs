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
        Task<IList<Form>> GetByCompanyAsync(string companyId);
        Task<FormMetadataDTO> GetMetadataByIdAsync(string id);
        Task CreateAsync(Form form);
        Task<ValidationResultDTO<Form>> ValidateToCreateAsync(string companyId, FormOnPostDTO dto);
        Task UpdateAsync(Form form);
        Task<ValidationResultDTO<Form>> ValidateToUpdateAsync(Form form, FormOnPutDTO dto);
    }
}
