using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IFormsService
    {
        Task<Form> GetFormAsync(string id);
        Task<FormMetadataDTO> GetMetadataAsync(string id);
        Task<Form> GenerateDefaultForm(Company company);
    }
}
