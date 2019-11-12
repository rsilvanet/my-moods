using MongoDB.Driver;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.Enums;
using System.Collections.Generic;

namespace MyMoods.Shared.Mongo
{
    public class Storage : IStorage
    {
        private readonly IMongoDatabase _database;

        public Storage(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Company> Companies => _database.GetCollection<Company>("companies");
        public IMongoCollection<Form> Forms => _database.GetCollection<Form>("forms");
        public IMongoCollection<Question> Questions => _database.GetCollection<Question>("questions");
        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("reviews");
        public IMongoCollection<Tagg> Tags => GetTags();
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");

        public IMongoCollection<Tagg> GetTags()
        {
            var collection = _database.GetCollection<Tagg>("tags");

            if (collection.EstimatedDocumentCount() == 0)
            {
                var tags = new List<Tagg>()
                {
                    new Tagg(TagType.realization, "Desafios"),
                    new Tagg(TagType.realization, "Autonomia"),
                    new Tagg(TagType.realization, "Participação em decisões"),
                    new Tagg(TagType.realization, "Perspectiva de crescimento"),
                    new Tagg(TagType.esteem, "Feedbacks"),
                    new Tagg(TagType.esteem, "Reconhecimento"),
                    new Tagg(TagType.esteem, "Responsabilidades"),
                    new Tagg(TagType.esteem, "Aumentos/Promoções"),
                    new Tagg(TagType.social, "Relação com diretores"),
                    new Tagg(TagType.social, "Relação com gestores"),
                    new Tagg(TagType.social, "Relação com colegas"),
                    new Tagg(TagType.social, "Relação com clientes"),
                    new Tagg(TagType.safety, "Estabilidade"),
                    new Tagg(TagType.safety, "Salário"),
                    new Tagg(TagType.safety, "Benefícios"),
                    new Tagg(TagType.physiological, "Conforto"),
                    new Tagg(TagType.physiological, "Local de trabalho"),
                    new Tagg(TagType.physiological, "Horário de trabalho"),
                    new Tagg(TagType.physiological, "Intervalos de descanso"),
                    new Tagg(TagType.physiological, "Equipamentos de trabalho")
                };

                foreach (var tag in tags)
                {
                    collection.InsertOne(tag);
                }
            }

            return collection;
        }
    }
}
