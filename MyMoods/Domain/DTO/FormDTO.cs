﻿namespace MyMoods.Domain.DTO
{
    public class FormDTO
    {
        public FormDTO(Form form, Company company)
        {
            Id = form.Id.ToString();
            Title = form.Title;
            Company = new CompanyDTO(company);
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public CompanyDTO Company { get; set; }

        public class CompanyDTO
        {
            public CompanyDTO(Company company)
            {
                Name = company.Name;
            }

            public string Name { get; set; }
        }
    }
}
