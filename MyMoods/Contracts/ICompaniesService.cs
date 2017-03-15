using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface ICompaniesService
    {
        Task<Company> GetByIdAsync(string id);
        Task InsertAsync(Company company);
        Task<ValidationResultDTO<Company>> ValidateToInsertAsync(RegisterDTO register);
    }
}
