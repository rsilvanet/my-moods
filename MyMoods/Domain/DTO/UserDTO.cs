using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            Id = user.Id.ToString();
            Name = user.Name;
            Email = user.Email;
            Companies = user.Companies.Select(x => x.ToString()).ToList();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IList<string> Companies { get; set; }
    }
}
