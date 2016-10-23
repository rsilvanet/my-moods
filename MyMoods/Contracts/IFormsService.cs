using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IFormsService
    {
        Task<FormMetadataDTO> GetMetadata(string id);
    }
}
