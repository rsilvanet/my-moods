using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class ValidationResultDTO
    {
        public ValidationResultDTO()
        {
            Errors = new Dictionary<string, string>();
            Alerts = new Dictionary<string, string>();
        }

        public bool Success => !Errors.Any();
        public IDictionary<string, string> Errors { get; set; }
        public IDictionary<string, string> Alerts { get; set; }

        public void Error(string key, string error)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors.Add(key, error);
            }
        }

        public void Alert(string key, string alert)
        {
            if (!Alerts.ContainsKey(key))
            {
                Alerts.Add(key, alert);
            }
        }
    }

    public class ValidationResultDTO<T> : ValidationResultDTO where T : new()
    {
        public ValidationResultDTO() : base()
        {
            ParsedObject = new T();
        }

        public T ParsedObject { get; set; }
    }
}
