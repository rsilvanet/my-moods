namespace MyMoods.Shared.Domain.DTO
{
    public class CompanyDTO
    {
        public CompanyDTO(Company company)
        {
            Name = company.Name;
            Logo = company.Logo;
        }

        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
