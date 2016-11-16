using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
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

        public async Task InsertAsync(Company company)
        {
            await _storage.Companies.InsertOneAsync(company);
        }

        public async Task<ValidationResultDTO<Company>> ValidateToInsertAsync(RegisterDTO register)
        {
            var result = new ValidationResultDTO<Company>();

            result.ParsedObject.RegisterDate = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(register.Company))
            {
                result.Error("company", "O nome da empresa não foi informado.");
            }
            else
            {
                var company = await _storage.Companies.Find(x => x.Name.ToLower() == register.Company.ToLower()).FirstOrDefaultAsync();

                //TODO: Is it needed?
                //if (company != null)
                //{
                //    result.Error("company", "Já existe uma empresa cadastrada com este nome.");
                //}
                //else
                //{
                    result.ParsedObject.Name = register.Company;
                //}
            }

            return result;
        }
    }
}
