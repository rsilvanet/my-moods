using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class ValidationResultDTO<T> where T : new()
    {
        public ValidationResultDTO()
        {
            Errors = new Dictionary<string, string>();
            Alerts = new Dictionary<string, string>();
            ParsedObject = new T();
        }

        public bool Success => !Errors.Any();
        public IDictionary<string, string> Errors { get; set; }
        public IDictionary<string, string> Alerts { get; set; }
        public T ParsedObject { get; set; }

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
}
