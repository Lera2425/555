using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSearchApp
{
    public partial class JobSearchForm : Form
    {
        public JobSearchForm()
        {
            InitializeComponent();
        }

        private void JobSearchForm_Load(object sender, EventArgs e)
        {
            using System;
            using System.Collections.Generic;
            using System.Windows.Forms;

namespace JobSearchApp
    {
        public partial class JobSearchForm : Form
        {
            private List<Resume> resumes = new List<Resume>();
            private List<JobListing> jobs = new List<JobListing>();
            private Resume selectedResume = null;

            // Элементы управления
            private ListBox lstResumes, lstJobs;
            private TextBox txtSearch;
            private Label lblStatus;

            public JobSearchForm()
            {
                Text = "Управление резюме и поиском работы";
                Size = new System.Drawing.Size(950, 550);
                StartPosition = FormStartPosition.CenterScreen;
                InitializeUI();
            }

            private void InitializeUI()
            {
                // ========== ЛЕВАЯ ПАНЕЛЬ - РЕЗЮМЕ ==========
                var lblResumes = new Label
                {
                    Text = "=== МОИ РЕЗЮМЕ ===",
                    Location = new System.Drawing.Point(10, 10),
                    Width = 440,
                    Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
                };

                lstResumes = new ListBox
                {
                    Location = new System.Drawing.Point(10, 40),
                    Width = 440,
                    Height = 200
                };
                lstResumes.SelectedIndexChanged += (s, e) =>
                {
                    if (lstResumes.SelectedItem != null)
                        selectedResume = (Resume)lstResumes.SelectedItem;
                };

                // Кнопки для резюме
                int y = 255;
                AddButton("1. СОЗДАТЬ РЕЗЮМЕ", 10, y, 210, 35, System.Drawing.Color.LightGreen, (s, e) => CreateResume());
                AddButton("2. ДОБАВИТЬ НАВЫК", 230, y, 210, 35, System.Drawing.Color.White, (s, e) => AddSkill());

                y += 45;
                AddButton("3. ДОБАВИТЬ ОПЫТ", 10, y, 210, 35, System.Drawing.Color.White, (s, e) => AddWork());
                AddButton("4. ДОБАВИТЬ ОБРАЗОВАНИЕ", 230, y, 210, 35, System.Drawing.Color.White, (s, e) => AddEducation());

                y += 45;
                AddButton("5. ПОКАЗАТЬ РЕЗЮМЕ", 10, y, 210, 35, System.Drawing.Color.LightBlue, (s, e) => ShowResume());
                AddButton("6. УДАЛИТЬ РЕЗЮМЕ", 230, y, 210, 35, System.Drawing.Color.LightCoral, (s, e) => DeleteResume());

                // ========== ПРАВАЯ ПАНЕЛЬ - ВАКАНСИИ ==========
                var lblJobs = new Label
                {
                    Text = "=== ВАКАНСИИ ===",
                    Location = new System.Drawing.Point(470, 10),
                    Width = 440,
                    Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold)
                };

                txtSearch = new TextBox
                {
                    Location = new System.Drawing.Point(470, 40),
                    Width = 340
                };
                txtSearch.TextChanged += (s, e) => SearchJobs();

                var lblSearch = new Label
                {
                    Text = "Поиск:",
                    Location = new System.Drawing.Point(470, 65),
                    AutoSize = true
                };

                lstJobs = new ListBox
                {
                    Location = new System.Drawing.Point(470, 90),
                    Width = 440,
                    Height = 200
                };
                lstJobs.DoubleClick += (s, e) => ShowJobDetails();

                // Кнопки для вакансий
                y = 305;
                AddButton("7. ДОБАВИТЬ ВАКАНСИЮ", 470, y, 210, 35, System.Drawing.Color.LightGreen, (s, e) => AddJob());
                AddButton("8. ДЕТАЛИ ВАКАНСИИ", 690, y, 210, 35, System.Drawing.Color.LightBlue, (s, e) => ShowJobDetails());

                y += 45;
                AddButton("9. УДАЛИТЬ ВАКАНСИЮ", 470, y, 210, 35, System.Drawing.Color.LightCoral, (s, e) => DeleteJob());
                AddButton("10. ОБНОВИТЬ СПИСОК", 690, y, 210, 35, System.Drawing.Color.LightGray, (s, e) => SearchJobs());

                // Статус бар
                lblStatus = new Label
                {
                    Text = "Готов",
                    Location = new System.Drawing.Point(10, 470),
                    Width = 900,
                    Height = 30,
                    BackColor = System.Drawing.Color.LightGray,
                    Padding = new Padding(5)
                };

                // Добавляем все элементы на форму
                Controls.Add(lblResumes);
                Controls.Add(lstResumes);
                Controls.Add(lblJobs);
                Controls.Add(txtSearch);
                Controls.Add(lblSearch);
                Controls.Add(lstJobs);
                Controls.Add(lblStatus);
            }

            private void AddButton(string text, int x, int y, int w, int h, System.Drawing.Color color, EventHandler click)
            {
                var btn = new Button
                {
                    Text = text,
                    Location = new System.Drawing.Point(x, y),
                    Width = w,
                    Height = h,
                    BackColor = color,
                    UseVisualStyleBackColor = false
                };
                btn.Click += click;
                Controls.Add(btn);
            }

            private void UpdateStatus(string message)
            {
                lblStatus.Text = $"Статус: {message}";
            }

            // ========== МЕТОДЫ РАБОТЫ С РЕЗЮМЕ ==========

            private void CreateResume()
            {
                string name = InputBox("Создание резюме", "Введите имя:", "");
                if (string.IsNullOrWhiteSpace(name)) return;

                string contact = InputBox("Создание резюме", "Введите контактную информацию:", "");
                string objective = InputBox("Создание резюме", "Введите цель резюме:", "");

                var resume = new Resume(name, contact, objective);
                resumes.Add(resume);
                UpdateResumeList();
                UpdateStatus($"Резюме '{name}' создано");
            }

            private void AddSkill()
            {
                if (!CheckResumeSelected()) return;

                string skill = InputBox("Добавление навыка", "Введите название навыка:", "");
                if (!string.IsNullOrWhiteSpace(skill))
                {
                    selectedResume.AddSkill(skill);
                    UpdateStatus($"Навык '{skill}' добавлен");
                }
            }

            private void AddWork()
            {
                if (!CheckResumeSelected()) return;

                string position = InputBox("Опыт работы", "Введите должность:", "");
                if (string.IsNullOrWhiteSpace(position)) return;

                string company = InputBox("Опыт работы", "Введите компанию:", "");
                string period = InputBox("Опыт работы", "Введите период (например: 2020-2023):", "");
                string description = InputBox("Опыт работы", "Введите описание обязанностей:", "");

                selectedResume.AddWorkExperience(position, company, period, description);
                UpdateStatus($"Опыт '{position}' добавлен");
            }

            private void AddEducation()
            {
                if (!CheckResumeSelected()) return;

                string institution = InputBox("Образование", "Введите учебное заведение:", "");
                if (string.IsNullOrWhiteSpace(institution)) return;

                string degree = InputBox("Образование", "Введите степень/специальность:", "");
                string period = InputBox("Образование", "Введите период обучения:", "");

                selectedResume.AddEducation(institution, degree, period);
                UpdateStatus($"Образование в '{institution}' добавлено");
            }

            private void ShowResume()
            {
                if (!CheckResumeSelected()) return;

                MessageBox.Show(selectedResume.GetFullInfo(), $"Резюме: {selectedResume.Name}",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            private void DeleteResume()
            {
                if (!CheckResumeSelected()) return;

                if (MessageBox.Show($"Удалить резюме '{selectedResume.Name}'?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    resumes.Remove(selectedResume);
                    selectedResume = null;
                    UpdateResumeList();
                    UpdateStatus("Резюме удалено");
                }
            }

            private bool CheckResumeSelected()
            {
                if (selectedResume == null && lstResumes.SelectedItem != null)
                    selectedResume = (Resume)lstResumes.SelectedItem;

                if (selectedResume == null)
                {
                    MessageBox.Show("Сначала выберите или создайте резюме!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }

            private void UpdateResumeList()
            {
                lstResumes.Items.Clear();
                foreach (var r in resumes)
                    lstResumes.Items.Add(r);

                if (lstResumes.Items.Count > 0)
                    lstResumes.SelectedIndex = 0;
            }

            // ========== МЕТОДЫ РАБОТЫ С ВАКАНСИЯМИ ==========

            private void AddJob()
            {
                string title = InputBox("Добавление вакансии", "Введите название вакансии:", "");
                if (string.IsNullOrWhiteSpace(title)) return;

                string company = InputBox("Добавление вакансии", "Введите компанию:", "");
                string description = InputBox("Добавление вакансии", "Введите описание:", "");
                string requirements = InputBox("Добавление вакансии", "Введите требования:", "");

                jobs.Add(new JobListing(title, company, description, requirements));
                SearchJobs();
                UpdateStatus($"Вакансия '{title}' добавлена");
            }

            private void DeleteJob()
            {
                if (lstJobs.SelectedItem == null)
                {
                    MessageBox.Show("Выберите вакансию для удаления!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var job = (JobListing)lstJobs.SelectedItem;
                if (MessageBox.Show($"Удалить вакансию '{job.JobTitle}'?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    jobs.Remove(job);
                    SearchJobs();
                    UpdateStatus("Вакансия удалена");
                }
            }

            private void SearchJobs()
            {
                string query = txtSearch.Text.ToLower();
                lstJobs.Items.Clear();

                foreach (var job in jobs)
                {
                    if (string.IsNullOrEmpty(query) ||
                        job.JobTitle.ToLower().Contains(query) ||
                        job.Company.ToLower().Contains(query))
                    {
                        lstJobs.Items.Add(job);
                    }
                }

                UpdateStatus($"Найдено {lstJobs.Items.Count} вакансий");
            }

            private void ShowJobDetails()
            {
                if (lstJobs.SelectedItem == null)
                {
                    MessageBox.Show("Выберите вакансию для просмотра!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var job = (JobListing)lstJobs.SelectedItem;
                MessageBox.Show(job.GetFullInfo(), $"Вакансия: {job.JobTitle}",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // ========== ДИАЛОГОВОЕ ОКНО ДЛЯ ВВОДА ==========

            private string InputBox(string title, string prompt, string defaultValue)
            {
                Form form = new Form();
                Label label = new Label();
                TextBox textBox = new TextBox();
                Button buttonOk = new Button();
                Button buttonCancel = new Button();

                form.Text = title;
                label.Text = prompt;
                textBox.Text = defaultValue;

                buttonOk.Text = "OK";
                buttonCancel.Text = "Отмена";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                label.SetBounds(9, 20, 372, 13);
                textBox.SetBounds(12, 36, 372, 20);
                buttonOk.SetBounds(228, 72, 75, 23);
                buttonCancel.SetBounds(309, 72, 75, 23);

                label.AutoSize = true;
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new System.Drawing.Size(396, 107);
                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                if (form.ShowDialog() == DialogResult.OK)
                    return textBox.Text;
                else
                    return "";
            }
        }
    }

}
    }
}
