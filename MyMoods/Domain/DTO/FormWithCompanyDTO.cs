namespace MyMoods.Domain.DTO
{
    public class FormWithCompanyDTO : FormDTO
    {
        public FormWithCompanyDTO(Form form, Company company) : base(form)
        {
            Company = new CompanyDTO(company);
        }

        public CompanyDTO Company { get; set; }
    }
}
