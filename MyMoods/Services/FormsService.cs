using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class FormsService : IFormsService
    {
        private readonly IStorage _storage;
        private readonly IMoodsService _moodsService;

        public FormsService(IStorage storage, IMoodsService moodsService)
        {
            _storage = storage;
            _moodsService = moodsService;
        }

        public async Task<Form> GetFormAsync(string id)
        {
            var oid = new ObjectId(id);
            var form = await _storage.Forms.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            return form;
        }

        public async Task<FormMetadataDTO> GetMetadataAsync(string id)
        {
            var oid = new ObjectId(id);
            var form = await _storage.Forms.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();
            var company = await _storage.Companies.Find(x => x.Id.Equals(form.Company)).FirstOrDefaultAsync();
            var questions = await _storage.Questions.Find(x => x.Form.Equals(oid)).ToListAsync();
            var tags = await _storage.Tags.Find(x => true).ToListAsync();
            var moods = _moodsService.GetMoods();

            return new FormMetadataDTO(form, company, questions, tags, moods);
        }
    }
}
