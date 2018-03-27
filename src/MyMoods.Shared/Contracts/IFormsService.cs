using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Shared.Contracts
{
    public interface IFormsService
    {
        Task<Form> GetByIdAsync(string id, bool loadTags = false, bool loadQuestions = false);
        Task<IList<Form>> GetByCompanyAsync(string companyId, bool onlyActives);
        Task<FormMetadataDTO> GetMetadataByIdAsync(string id);
        Task<IList<Question>> GetQuestionsAsync(Form form);
        Task InsertAsync(Form form);
        Task UpdateAsync(Form form);
        Task EnableAsync(Form form);
        Task DisableAsync(Form form);
        Task<ValidationResultDTO<Form>> ValidateToInsertAsync(string companyId, FormOnPostDTO dto);
        Task<ValidationResultDTO<Form>> ValidateToUpdateAsync(Form form, FormOnPutDTO dto);
        Task SendReminderAsync(string id);
        Task EnqueueReminderAsync(NotificationRecurrence recurrence);
    }
}
