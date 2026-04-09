using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JobSearchApp
{
    public class Resume
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Objective { get; set; }
        public List<string> Skills { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; }
        public List<Education> Educations { get; set; }

        public Resume(string name, string contactInfo, string objective)
        {
            Name = name;
            ContactInfo = contactInfo;
            Objective = objective;
            Skills = new List<string>();
            WorkExperiences = new List<WorkExperience>();
            Educations = new List<Education>();
        }

        public void AddSkill(string skill)
        {
            Skills.Add(skill);
            MessageBox.Show($"Навык '{skill}' добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void AddWorkExperience(string position, string company, string period, string description)
        {
            WorkExperiences.Add(new WorkExperience(position, company, period, description));
            MessageBox.Show($"Опыт работы '{position}' в '{company}' добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void AddEducation(string institution, string degree, string period)
        {
            Educations.Add(new Education(institution, degree, period));
            MessageBox.Show($"Образование в '{institution}' добавлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string GetFullInfo()
        {
            string result = $"Имя: {Name}\n";
            result += $"Контакты: {ContactInfo}\n";
            result += $"Цель: {Objective}\n\n";

            result += "=== НАВЫКИ ===\n";
            foreach (var s in Skills) result += $"• {s}\n";

            result += "\n=== ОПЫТ РАБОТЫ ===\n";
            foreach (var w in WorkExperiences) result += $"• {w}\n";

            result += "\n=== ОБРАЗОВАНИЕ ===\n";
            foreach (var e in Educations) result += $"• {e}\n";

            return result;
        }

        public override string ToString() => $"{Name} - {Objective}";
    }

    public class WorkExperience
    {
        public string Position { get; set; }
        public string Company { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }

        public WorkExperience(string position, string company, string period, string description)
        {
            Position = position;
            Company = company;
            Period = period;
            Description = description;
        }

        public override string ToString() => $"{Position} в {Company} ({Period})";
    }

    public class Education
    {
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string Period { get; set; }

        public Education(string institution, string degree, string period)
        {
            Institution = institution;
            Degree = degree;
            Period = period;
        }

        public override string ToString() => $"{Institution} - {Degree} ({Period})";
    }
}