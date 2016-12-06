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
        Task<Form> CreateFormAsync(string companyId, FormOnPostDTO dto);
        Task<ValidationResultDTO> ValidateToCreateFormAsync(FormOnPostDTO dto);
        Task UpdateFormAsync(Form form, FormOnPostDTO dto);
        Task<ValidationResultDTO> ValidateToUpdateFormAsync(FormOnPostDTO dto);
    }
}
