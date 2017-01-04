using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;
using System;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Form : Entity
    {
        private IList<Question> _questions;

        public Form()
        {
            Active = true;
            CustomTags = new List<ObjectId>();
        }

        public Form(ObjectId company)
        {
            Active = true;
            Company = company;
            CustomTags = new List<ObjectId>();
        }

        public bool Active { get; set; }
        public string Title { get; set; }
        public string MainQuestion { get; set; }
        public FormType Type { get; set; }
        public IList<ObjectId> CustomTags { get; set; }
        public ObjectId Company { get; set; }
        public bool RequireTagsForReviews => Type != FormType.simple;

        public IEnumerable<Question> Questions
        {
            get
            {
                if (_questions == null)
                {
                    throw new Exception("As questões do formulário não foram carregadas.");
                }

                return _questions;
            }
        }

        public void AddQuestion(Question question)
        {
            if (_questions == null)
            {
                _questions = new List<Question>();
            }

            _questions.Add(question);
        }

        public void LoadQuestions(IList<Question> questions = null)
        {
            _questions = questions ?? new List<Question>();
        }
    }
}
