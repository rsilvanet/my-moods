using MyMoods.Contracts;
using MyMoods.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class TagsService
    {
        private readonly IStorage _storage;

        public TagsService(IStorage storage)
        {
            _storage = storage;
        }
    }
}
