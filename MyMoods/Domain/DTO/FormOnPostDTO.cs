using System.Collections.Generic;

namespace MyMoods.Domain.DTO
{
    public class FormOnPostDTO : FormOnPutDTO
    {
        public string Title { get; set; }
        public FormType? Type { get; set; }
    }
}
