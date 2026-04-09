using System;

namespace JobSearchApp
{
    public class JobListing
    {
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }

        public JobListing(string jobTitle, string company, string description, string requirements)
        {
            JobTitle = jobTitle;
            Company = company;
            Description = description;
            Requirements = requirements;
        }

        public string GetFullInfo()
        {
            return $"Вакансия: {JobTitle}\nКомпания: {Company}\nОписание: {Description}\nТребования: {Requirements}";
        }

        public override string ToString() => $"{JobTitle} в {Company}";
    }
}