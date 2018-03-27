using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Shared.Contracts
{
    public interface ICompaniesService
    {
        Task<Company> GetByIdAsync(string id);
        Task InsertAsync(Company company);
        Task<ValidationResultDTO<Company>> ValidateToInsertAsync(RegisterDTO register);
    }
}
