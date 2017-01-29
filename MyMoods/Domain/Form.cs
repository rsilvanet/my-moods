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
        private IList<Tagg> _tags;
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
        public bool TagsAreLoaded => _tags != null;
        public bool QuestionsAreLoaded => _questions != null;

        #region Tags

        public IEnumerable<Tagg> Tags
        {
            get
            {
                if (_tags == null)
                {
                    throw new Exception("As tags do formulário não foram carregadas.");
                }

                return _tags;
            }
        }

        public void AddTag(Tagg tag)
        {
            if (_tags == null)
            {
                _tags = new List<Tagg>();
            }

            _tags.Add(tag);
        }

        public void LoadTags(IList<Tagg> tags = null)
        {
            _tags = tags ?? new List<Tagg>();
        }

        #endregion

        #region Questions

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

        #endregion
    }
}
