using MyMoods.Shared.Domain.Enums;

namespace MyMoods.Shared.Domain.DTO
{
    public class FormOnPostDTO : FormOnPutDTO
    {
        public string Title { get; set; }
        public FormType? Type { get; set; }
    }
}
