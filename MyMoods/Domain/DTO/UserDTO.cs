﻿using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            Id = user.ToString();
            Email = user.Email;
            Password = user.Password;
            Companies = user.Companies.Select(x => x.ToString()).ToList();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<string> Companies { get; set; }
    }
}
