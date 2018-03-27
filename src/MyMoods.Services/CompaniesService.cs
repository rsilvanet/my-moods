using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly IStorage _storage;

        public CompaniesService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<Company> GetByIdAsync(string id)
        {
            var oid = new ObjectId(id);
            var company = await _storage.Companies.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            return company;
        }

        public async Task InsertAsync(Company company)
        {
            await _storage.Companies.InsertOneAsync(company);
        }

        public Task<ValidationResultDTO<Company>> ValidateToInsertAsync(RegisterDTO register)
        {
            var result = new ValidationResultDTO<Company>();

            if (string.IsNullOrWhiteSpace(register.Company))
            {
                result.Error("company", "O nome da empresa não foi informado.");
            }
            else
            {
                result.ParsedObject.Name = register.Company;
            }

            return Task.FromResult(result);
        }
    }
}
